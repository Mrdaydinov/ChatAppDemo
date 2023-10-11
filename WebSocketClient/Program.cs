using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        
        var serverUri = new Uri("ws://localhost:5239/ws"); 
        
        
        var client = new ClientWebSocket();

        try
        {
            await client.ConnectAsync(serverUri, CancellationToken.None);

            Console.WriteLine("WebSocket connected!");

            Task receiveTask = Receive(client);
            Task sendTask = Send(client);

            await Task.WhenAll(receiveTask, sendTask);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket error: {ex.Message}");
        }
        finally
        {
            if (client.State == WebSocketState.Open)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", CancellationToken.None);
            }
        }
    }

    static async Task Receive(ClientWebSocket client)
    {
        try
        {
            var buffer = new byte[1024];
            while (true)
            {
                WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine("Received: " + message);
                }
            }
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine("WebSocket closed: " + ex.Message);
        }
    }

    static async Task Send(ClientWebSocket client)
    {
        try
        {
            while (true)
            {
                //Console.Write("Enter message: ");
                string message = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine("WebSocket closed: " + ex.Message);
        }
    }
}
