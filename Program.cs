using Microsoft.AspNetCore.SignalR;
using RealtimeChatDemo.Hubs;

var builder = WebApplication.CreateBuilder(args);

// --- Services -----------------------------------------------------------

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Loosened for demo purposes so you can host the static UI anywhere
// (e.g. a different domain/port) and still connect to the hub.
// Tighten this to your real front-end origin before using in production.
builder.Services.AddCors(options =>
{
    options.AddPolicy("DemoCors", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed(_ => true)
              .AllowCredentials();
    });
});

var app = builder.Build();

// --- Middleware -----------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();   // serves wwwroot/index.html at "/"
app.UseStaticFiles();
app.UseCors("DemoCors");

// --- Endpoints ------------------------------------------------------------

app.MapHub<ChatHub>("/hubs/chat");

// A plain REST endpoint that pushes a real-time notification.
// This is the piece that maps directly to a real POS/business scenario:
// e.g. a "place order" API call that also instantly notifies every
// connected terminal/dashboard via SignalR - no polling required.
app.MapPost("/api/notifications", async (
    NotificationRequest request,
    IHubContext<ChatHub> hubContext) =>
{
    await hubContext.Clients.Group(request.Room).SendAsync(
        "ReceiveNotification",
        request.Title,
        request.Body,
        DateTime.UtcNow.ToString("HH:mm:ss"));

    return Results.Ok(new { status = "sent" });
})
.WithName("SendNotification")
.WithOpenApi();

app.MapGet("/api/health", () => Results.Ok(new { status = "healthy" }))
   .WithName("HealthCheck")
   .WithOpenApi();

app.Run();

record NotificationRequest(string Room, string Title, string Body);
