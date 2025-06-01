namespace NativeChat;

public interface IMessagesService
{
    public Task<List<Message>> GetAllMessages(Guid botId, Guid userId);

    public Task<Message?> SendMessageAsync(Guid userId, Message message);
}
