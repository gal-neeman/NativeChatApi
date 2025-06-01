using Microsoft.EntityFrameworkCore;

namespace NativeChat;

public class ValidationService : IValidationService
{
    private readonly NativeChatContext _db;

    public ValidationService(NativeChatContext db)
    {
        _db = db;
    }

    public async Task<bool> IsEmailTakenAsync(string email)
    {
        return await _db.Users.AnyAsync(u => u.Email == email.ToLower());
    }

    public async Task<bool> IsUserExistsAsync(CredentialsDto user)
    {
        var password = Encryptor.GetHashed(user.Password);
        return await _db.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower() && u.Password == password);
    }

    public async Task<bool> IsUserExistsAsync(Guid userId)
    {
        return await _db.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<bool> IsBotExistsAsync(Guid botId)
    {
        return await _db.Bots.AnyAsync(b => b.Id == botId);
    }
}
