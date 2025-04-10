using Microsoft.EntityFrameworkCore;

namespace NativeChat;

public class UserDao : IUserDao
{
    private readonly NativeChatContext _db;

    public UserDao(NativeChatContext db)
    {
        _db = db;
    }

    public async Task<User> AddUserAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetUserAsync(CredentialsDto user)
    {
        string password = Encryptor.GetHashed(user.Password);

        return await _db.Users.SingleAsync(u => u.Email == user.Email && u.Password == password);
    }
}
