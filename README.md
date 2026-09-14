# Real-Time Backend Demo (ASP.NET Core + SignalR)

Two things happen live, in every open browser tab:
1. Chat messages (client → hub → all clients in the room)
2. Server-pushed notifications (REST API call → hub → all clients) —
   this is the same pattern used for "new order received" on a POS system,
   "stock running low" alerts, or a live dashboard.


live url link : https://nazam-realtime-chat-frabcnd4esdzgeaz.westus3-01.azurewebsites.net/