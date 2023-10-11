using System.Collections.Concurrent;

namespace ChatAppDemo;

public class ChatHandler : WebSocketHandler
{
   public ChatHandler(ConnectionManager connectionManager) : base(connectionManager) {}
   
}