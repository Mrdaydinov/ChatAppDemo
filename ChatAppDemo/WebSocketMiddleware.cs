using System.Net.WebSockets;

namespace ChatAppDemo;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    
    private WebSocketHandler _webSocketHandler { get; set; }

    public WebSocketMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler)
    {
        _next = next;
        _webSocketHandler = webSocketHandler;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            await _next.Invoke(context);
            return;
        }

        WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();

        _webSocketHandler.OnConnected(socket);

        var buffer = new byte[1024 * 4];
        var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
        var name = _webSocketHandler.GetName(socket, result, buffer);
        await _webSocketHandler.AddContactAsync(name, socket);

        await Receive(socket, async (result, buffer) =>
        {
            if (result.MessageType == WebSocketMessageType.Text)
            {
                 await _webSocketHandler.ReceiveAsync(socket, result, buffer);
                return;
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await _webSocketHandler.OnDisconnected(socket);
                return;
            }
        });
    }

    public async Task Receive(WebSocket socket, Action<WebSocketReceiveResult,byte[]> handleMessage)
    {
        var buffer = new byte[1024 * 4];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            
            handleMessage(result,buffer);
        }
    }
}