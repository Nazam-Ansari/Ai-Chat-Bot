# Real-Time Backend Demo (ASP.NET Core + SignalR)

Two things happen live, in every open browser tab:
1. Chat messages (client → hub → all clients in the room)
2. Server-pushed notifications (REST API call → hub → all clients) —
   this is the same pattern used for "new order received" on a POS system,
   "stock running low" alerts, or a live dashboard.

## Run it locally

You need the [.NET 8 SDK](https://dotnet.microsoft.com/download) installed.

```bash
cd RealtimeChatDemo
dotnet run
```

Then open the URL it prints (usually `http://localhost:5000` or similar)
in **two separate browser tabs**. Type a message in one tab — it appears
in both instantly. Click "Simulate server notification" — both tabs get
the alert at the same time.

Swagger UI (to show clients you document your APIs properly) is available
at `/swagger` when running in Development mode.

## Deploy it for free (so you have a live link to share with clients)

Pick one:

- **Azure App Service (Free F1 tier)** — natural fit since it's .NET;
  Microsoft Learn has a step-by-step "Deploy an ASP.NET Core app to
  Azure App Service" guide.
- **Railway** — connect your GitHub repo, it detects the Dockerfile/SDK
  and deploys automatically. Simple free tier for small demos.
- **Render** — similar to Railway, free tier available for small web
  services (may sleep after inactivity, which is fine for a portfolio demo).

Once deployed, put the live link in:
- Your Fiverr gig description ("see it live: ...")
- Your Fiverr portfolio section (with a screenshot + the link)
- Your GitHub README for this repo

## How to relabel this for different clients

The underlying code doesn't need to change — just the labels and pitch:

| Pitch to client as...         | What you change                                |
|--------------------------------|-------------------------------------------------|
| Live customer support chat     | Rename "room" to "ticket ID", style the UI      |
| POS "new order" notifications  | Call `/api/notifications` from an actual order-creation endpoint |
| Live admin dashboard           | Replace the chat log with a data table that updates on `ReceiveNotification` |

## Next steps to make this even stronger

- Add a simple `Orders` REST endpoint (POST /api/orders) that saves to
  SQL Server/EF Core AND triggers the SignalR notification — this shows
  the full "API + database + real-time" stack in one demo, closest to
  your actual Eratech POS experience.
- Add JWT authentication to the hub connection to show you know secure
  real-time auth, not just the happy path.
- Record a 30–60 second screen recording (Loom, free) of two tabs syncing
  live, and link that video in your Fiverr gig — video converts better
  than screenshots for technical buyers who don't want to test it themselves.
