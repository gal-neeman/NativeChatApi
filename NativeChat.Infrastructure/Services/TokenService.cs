using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NativeChat;

public class TokenService : ITokenService
{
    private readonly AuthSettings _authSettings;

    private readonly JwtSecurityTokenHandler _handler;
    private readonly SymmetricSecurityKey _symmetricSecurityKey; // Must be minimum 16 char string.

    public TokenService(IOptions<AuthSettings> authSettings)
    {
        _authSettings = authSettings.Value;
        _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Secret));
        _handler = new JwtSecurityTokenHandler();
    }

    public string GetNewToken(User user)
    {
        var userObject = new Dictionary<string, object>
        {
            { "id", user.Id.ToString() },
            { "firstname", user.FirstName },
            { "lastname", user.LastName },
            { "username", user.Username },
            { "email", user.Email },
        };

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddHours(_authSettings.JwtExpireHours),
            SigningCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512),
            Claims = new Dictionary<string, object>
            {
                { "user", userObject },
                { JwtRegisteredClaimNames.Aud, _authSettings.Audience } // Since this could be a list, add it as a custom claim
            },
            Issuer = _authSettings.Issuer,
        };

        SecurityToken securityToken = _handler.CreateToken(descriptor);
        string token = _handler.WriteToken(securityToken);
        return token;
    }
}
