using System.Net;

namespace NativeChat;

public interface IUserService
{
    public Task<string?> RegisterAsync(RegisterDto user);

    public Task<string?> LoginAsync(CredentialsDto credentials);
}
