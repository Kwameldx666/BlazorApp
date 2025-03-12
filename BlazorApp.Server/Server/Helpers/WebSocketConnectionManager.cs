using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace BlazorApp.Server.Helpers
{
    // WebSocketConnectionManager.cs
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _connections = new();

        public string AddConnection(WebSocket socket)
        {
            string connectionId = Guid.NewGuid().ToString();
            _connections.TryAdd(connectionId, socket);
            return connectionId;
        }

        public async Task RemoveConnection(string connectionId)
        {
            if (_connections.TryRemove(connectionId, out var socket))
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    "Closed by manager",
                    CancellationToken.None);
            }
        }

        public async Task BroadcastMessageAsync(string message)
        {
            foreach (var connection in _connections)
            {
                if (connection.Value.State == WebSocketState.Open)
                {
                    await SendMessageAsync(connection.Value, message);
                }
            }
        }

        private async Task SendMessageAsync(WebSocket socket, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }

        public async Task HandleWebSocketAsync(WebSocket socket)
        {
            var connectionId = AddConnection(socket);
            var buffer = new byte[1024 * 4];

            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer),
                        CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await RemoveConnection(connectionId);
                    }
                }
            }
            catch (Exception)
            {
                await RemoveConnection(connectionId);
            }
        }
    }
}
