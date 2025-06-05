namespace NativeChat;

public class UserCheckupSettings
{
    public int CheckupInterval { get; set; }

    public int MaxRandomMinutes { get; set; }

    public string CheckupMessage { get; set; } = null!;
}
