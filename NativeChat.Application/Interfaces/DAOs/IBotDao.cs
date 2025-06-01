namespace NativeChat;

public interface IBotDao
{
    public Task<List<Bot>> GetBotsByUserIdAsync(Guid userId);

    public Task DeleteBotAsync(Guid botId);

    public Task<Bot> AddBotAsync(Bot bot);

    public Task<Bot?> GetBotByIdAsync(Guid botId);
}
