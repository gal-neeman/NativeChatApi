using Microsoft.EntityFrameworkCore;

namespace NativeChat;

public class MessageDao : IMessageDao
{
    private readonly NativeChatContext _db;

    public MessageDao(NativeChatContext db)
    {
        _db = db;
    }

    public async Task<List<Message>> GetAllMessages(Guid botId, Guid userId)
    {
        return await _db.Messages.AsNoTracking()
            .Where(
            m => 
                (m.ReceiverId == botId && m.SenderId == userId) ||
                (m.ReceiverId == userId && m.SenderId == botId)
            )
            .OrderBy(m => m.CreatedAt).ToListAsync();
    }

    public async Task SendMessageAsync(Message message)
    {
        await _db.Messages.AddAsync(message);
        await _db.SaveChangesAsync();
    }

    public async Task SendMessagesAsync(ICollection<Message> messages)
    {
        foreach (var message in messages)
            await _db.Messages.AddAsync(message);

        await _db.SaveChangesAsync();
    }

    public async Task<Message?> GetLastMessageAsync(Guid userId, Guid botId)
    {
        return await _db.Messages.Where(m => (m.SenderId == userId && m.ReceiverId == botId) || (m.SenderId == botId && m.ReceiverId == userId)).OrderByDescending(m => m.CreatedAt).FirstOrDefaultAsync();
    }
}
