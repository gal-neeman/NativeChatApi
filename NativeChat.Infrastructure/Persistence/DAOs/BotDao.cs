using Microsoft.EntityFrameworkCore;

namespace NativeChat;

public class BotDao : IBotDao
{
    private readonly NativeChatContext _db;

    public BotDao(NativeChatContext db)
    {
        _db = db;
    }

    public async Task<List<Bot>> GetBotsByUserIdAsync(Guid userId)
    {
        return await _db.Bots.Include(b => b.Language).Where(b => b.UserId == userId).ToListAsync();
    }

    public async Task DeleteBotAsync(Guid botId)
    {
        var bot = (await _db.Bots.AsNoTracking().SingleOrDefaultAsync(b => b.Id == botId))!;
        _db.Bots.Remove(bot);
        await _db.SaveChangesAsync();
    }

    public async Task<Bot> AddBotAsync(Bot bot)
    {
        await _db.Bots.AddAsync(bot);
        await _db.SaveChangesAsync();

        return bot;
    } 

    public async Task<Bot?> GetBotByIdAsync(Guid botId)
    {
        return await _db.Bots.Include(b => b.Language).FirstOrDefaultAsync(b => b.Id == botId);
    }
}
