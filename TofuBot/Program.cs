

using System.Net;
using BotOneOne;
using BotOneOne.Connectivity;

var source = new ReversedWebSocketConnectionSource();
var ctx = new OneBotContext(source);

var listener = new HttpListener();
listener.Start();
while (true)
{
    var context = await listener.GetContextAsync();
    var webSocketCtx = await context.AcceptWebSocketAsync(null);
    source.UpgradeWebSocket(webSocketCtx.WebSocket);
}
