namespace NativeChat;

public interface IChatService
{
    public Task<Message> CompleteChatAsync(Message msg);
}
