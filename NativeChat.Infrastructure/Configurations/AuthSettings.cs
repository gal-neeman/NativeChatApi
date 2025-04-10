namespace NativeChat;

public class AuthSettings
{
    public string Secret { get; set; } = string.Empty;

    public int JwtExpireHours { get; set; }

    public string Issuer { get; set; } = string.Empty;

    public List<string> Audience { get; set; } = [];
}
