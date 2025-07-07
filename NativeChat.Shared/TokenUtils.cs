using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace NativeChat;

public static class TokenUtils
{
    public static string GetIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var json = handler.ReadJwtToken(token).Payload.SerializeToJson();

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        string id = root.GetProperty("user").GetProperty("id").GetString()!;

        return id;
    }
}
