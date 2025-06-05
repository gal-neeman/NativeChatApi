namespace NativeChat;

public interface IMessageDao
{
    public Task<List<Message>> GetAllMessages(Guid botId, Guid userId);

    public Task SendMessageAsync(Message message);

    public Task SendMessagesAsync(ICollection<Message> messages);

    public Task<Message?> GetLastMessageAsync(Guid userId, Guid botId);
}
