using Microsoft.EntityFrameworkCore;

namespace NativeChat;

public class ValidationService : IValidationService
{
    private readonly NativeChatContext _db;

    public ValidationService(NativeChatContext db)
    {
        _db = db;
    }

    public async Task<bool> IsEmailTaken(string email)
    {
        return await _db.Users.AnyAsync(u => u.Email == email.ToLower());
    }

    public async Task<bool> IsUserExistsAsync(CredentialsDto user)
    {
        var password = Encryptor.GetHashed(user.Password);
        return await _db.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower() && u.Password == password);
    }
}
