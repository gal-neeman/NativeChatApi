namespace NativeChat;

public interface IUserDao
{
    public Task<User> AddUserAsync(User user);

    public Task<User> GetUserAsync(CredentialsDto user);
}
