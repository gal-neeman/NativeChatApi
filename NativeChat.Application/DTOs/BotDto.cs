namespace NativeChat;

public class BotDto
{
    public Guid Id { get; set; }

    public Language Language { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateOnly CreatedAt { get; set; }
}
