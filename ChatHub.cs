using Microsoft.AspNetCore.SignalR;

namespace RealtimeChatDemo.Hubs;

/// <summary>
/// Central real-time hub. Demonstrates the core pattern clients pay for:
/// multiple connected users staying in sync instantly, without polling.
///
/// This is intentionally kept generic (rooms + messages + notifications)
/// so it can be relabeled for a portfolio piece as:
///   - a support/live-chat widget
///   - a POS "new order" notification feed
///   - a dashboard that pushes live updates to every connected screen
/// The underlying SignalR pattern is identical in all three.
/// </summary>
public class ChatHub : Hub
{
    // In-memory presence tracking. Fine for a demo; replace with a
    // proper store (Redis backplane) if you ever need multi-server scale.
    private static readonly Dictionary<string, string> ConnectionIdToUser = new();

    /// <summary>Client calls this once after connecting to register their display name.</summary>
    public async Task Register(string userName, string room)
    {
        ConnectionIdToUser[Context.ConnectionId] = userName;

        await Groups.AddToGroupAsync(Context.ConnectionId, room);

        await Clients.Group(room).SendAsync(
            "UserJoined",
            userName,
            DateTime.UtcNow.ToString("HH:mm:ss"));
    }

    /// <summary>Broadcasts a chat message to everyone in the same room, instantly.</summary>
    public async Task SendMessage(string room, string userName, string message)
    {
        await Clients.Group(room).SendAsync(
            "ReceiveMessage",
            userName,
            message,
            DateTime.UtcNow.ToString("HH:mm:ss"));
    }

    /// <summary>
    /// Example of a system-generated push notification (not user-typed) -
    /// this is the shape you'd reuse for "new order received", "stock low",
    /// "shift closed", etc. in a POS-style backend.
    /// </summary>
    public async Task SendNotification(string room, string title, string body)
    {
        await Clients.Group(room).SendAsync(
            "ReceiveNotification",
            title,
            body,
            DateTime.UtcNow.ToString("HH:mm:ss"));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (ConnectionIdToUser.TryGetValue(Context.ConnectionId, out var userName))
        {
            ConnectionIdToUser.Remove(Context.ConnectionId);
            await Clients.All.SendAsync(
                "UserLeft",
                userName,
                DateTime.UtcNow.ToString("HH:mm:ss"));
        }

        await base.OnDisconnectedAsync(exception);
    }
}
