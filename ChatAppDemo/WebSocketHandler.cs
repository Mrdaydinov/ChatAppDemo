using System.Net.WebSockets;
using System.Text;

namespace ChatAppDemo;

public abstract class WebSocketHandler
{
    protected ConnectionManager _connectionManager { get; set; }
    
    public WebSocketHandler(ConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public virtual async Task OnConnected(WebSocket socket)
    {
        _connectionManager.AddSocket(socket);
    }

    public virtual async Task OnDisconnected(WebSocket socket)
    {
         _connectionManager.RemoveSocket(socket);
    }


    public async Task SendMessageAsync(WebSocket socket, string message)
    {
        if (socket.State != WebSocketState.Open)
        {
            return;
        }
        
        var bytes = Encoding.ASCII.GetBytes(message);
        await socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
        
    }

    public async Task SendMessageAsycn(string socketId, string message)
    {
        await SendMessageAsync(_connectionManager.GetSocketById(socketId),message);
    }

    public async Task SendMessageToAllAsync(string message)
    {
        foreach (var socket in _connectionManager.GetAllSockets())
        {
            if (socket.Value.State == WebSocketState.Open)
            {
                await SendMessageAsync(socket.Value, message);
            }
        }
    }

    public async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
    {
        var socetId = _connectionManager.GetId(socket);
        var message = $"{socetId}  said {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
       
        await SendMessageToAllAsync(message);
    }
    
}