namespace NativeChat;

public interface IMessagesService
{
    public Task<List<Message>> GetAllMessages(Guid botId, Guid userId);

    public Task<MessageDto?> SendMessageAsync(Guid userId, Message message);
}
