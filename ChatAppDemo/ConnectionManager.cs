using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace ChatAppDemo;

public class ConnectionManager
{
    private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

    public WebSocket GetSocketById(string id)
    {
        return _sockets.FirstOrDefault(dict => dict.Key == id).Value;
    }

    public ConcurrentDictionary<string, WebSocket> GetAllSockets()
    {
        return _sockets;
    }

    
    
    public void AddSocket(WebSocket socket)
    {
        string socketId = CreateConnectionId();
        _sockets.TryAdd(socketId, socket);
    }

    public string GetId(WebSocket socket)
    {
        return _sockets.FirstOrDefault(dict => dict.Value == socket).Key;
    }

    private string CreateConnectionId()
    {
        return Guid.NewGuid().ToString();
    }

    // public async Task RemoveSocket(WebSocket socket, string description)
    // {
    //     string socketId = GetId(socket);
    //
    //     if (!string.IsNullOrEmpty(socketId))
    //     {
    //         _sockets.TryRemove(socketId,out _);
    //     }
    //
    //     if (socket.State != WebSocketState.Aborted)
    //     {
    //         await socket.CloseAsync(WebSocketCloseStatus.NormalClosure,description,CancellationToken.None);
    //     }
    // }
    
    public async Task RemoveSocket(WebSocket socket)
    {
        string socketId = GetId(socket);

        if (!string.IsNullOrEmpty(socketId))
        {
            _sockets.TryRemove(socketId,out _);
        }
        
    }
}