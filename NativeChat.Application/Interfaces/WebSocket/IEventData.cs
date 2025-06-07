using System.Text.Json.Serialization;

namespace NativeChat;

public class EventData<T>
{
    [JsonPropertyName("data")]
    public T Data { get; set; }

    [JsonPropertyName("eventName")]
    public string EventName { get; set; } = null!;
}
