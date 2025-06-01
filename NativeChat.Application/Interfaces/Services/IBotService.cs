namespace NativeChat;

public interface IBotService
{
    public Task<List<BotDto>?> GetUserBotsAsync(Guid userId);

    public Task<bool> DeleteBotAsync(Guid botId);

    public Task<BotDto> AddBotAsync(NewBotDto botDto, Guid userId);
}
