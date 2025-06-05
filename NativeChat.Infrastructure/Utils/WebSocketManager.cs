using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace NativeChat;

public static class WebSocketManager
{
    private static readonly ConcurrentDictionary<Guid, WebSocket> _sockets = new();

    public static IEnumerable<WebSocket> GetAll() => _sockets.Values;

    public static Guid? AddSocket(WebSocket socket, Guid id)
    {
        _sockets.TryAdd(id, socket);
        return id;
    }

    public static void RemoveSocket(Guid id)
    {
        _sockets.TryRemove(id, out var _);
    }

    public static void SendMessage(Guid targetId, Message message)
    {
        WebSocket socket = _sockets[targetId];
        string json = JsonSerializer.Serialize(message);
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        int a = 5;
    }
}
