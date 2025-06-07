using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace NativeChat;

public static class WebSocketManager
{
    private static readonly ConcurrentDictionary<Guid, WebSocket> _sockets = new();

    private static Dictionary<string, Func<dynamic, IMessagesService, Task>> _events = new Dictionary<string, Func<dynamic, IMessagesService, Task>>
    {
        ["message"] = async (message, messageService) =>
        {
            Message msg = (Message)message;
            Guid userId = msg.SenderId;
            MessageDto? response = await messageService.SendMessageAsync(userId, msg);

            if (response == null)
                return;

            WebSocket socket = _sockets[userId];
            EventData<MessageDto> eventData = new EventData<MessageDto>
            {
                Data = response,
                EventName = "message"
            };
            await SendMessageAsync(userId, eventData);
        }
    };

    public static IEnumerable<WebSocket> GetAll() => _sockets.Values;

    public static void AddSocket(WebSocket socket, Guid id)
    {
        if(_sockets.ContainsKey(id))
        {
            _sockets[id] = socket;
            return;
        }

        _sockets.TryAdd(id, socket);
    }

    public static void RemoveSocket(Guid id)
    {
        _sockets.TryRemove(id, out var _);
    }

    public async static Task SendMessageAsync(Guid socketId, object eventData)
    {
        WebSocket socket = _sockets[socketId];
        string json = JsonSerializer.Serialize(eventData);
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        await socket.SendAsync(bytes, WebSocketMessageType.Text, endOfMessage: true, cancellationToken: CancellationToken.None);
    }

    public async static Task HandleSocket(WebSocket socket, IMessagesService messagesService)
    {
        while (socket.State == WebSocketState.Open)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await socket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            if (receiveResult.MessageType == WebSocketMessageType.Close)
            {
                Guid socketId = _sockets.FirstOrDefault(s => ReferenceEquals(socket, s)).Key;
                RemoveSocket(socketId);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed connection", CancellationToken.None);
            }

            var eventDataJson = Encoding.UTF8.GetString(buffer).Trim(char.MinValue);
            var eventData = JsonSerializer.Deserialize<EventData<Message>>(eventDataJson);

            await _events[eventData!.EventName](eventData.Data, messagesService);
        }
    }
}
