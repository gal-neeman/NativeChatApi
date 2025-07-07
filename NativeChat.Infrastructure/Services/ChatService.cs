using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace NativeChat;

public class ChatService : IChatService
{
    private readonly ChatClient _chatClient;
    private readonly IOptions<OpenAISettings> _openAISettings;
    private readonly IBotDao _botDao;
    private readonly IMessageDao _messageDao;

    public ChatService(ChatClient chatClient, IOptions<OpenAISettings> openAISettings, IBotDao botDao, IMessageDao messageDao)
    {
        _chatClient = chatClient;
        _openAISettings = openAISettings;
        _botDao = botDao;
        _messageDao = messageDao;
    }

    public async Task<Message> CompleteChatAsync(Message message, Guid? userId = null)
    {
        Guid botId = (userId != null && userId == message.ReceiverId) ? message.SenderId : message.ReceiverId;
        Bot? bot = await _botDao.GetBotByIdAsync(botId);

        List<Message> previousMessages = await _messageDao.GetAllMessages(message.ReceiverId, message.SenderId);
        previousMessages.Add(message);
        List<ChatMessage> previousMessagesText = [ChatMessage.CreateSystemMessage(_openAISettings.Value.GetSystemMessage(bot!.Language.Name, bot!.Name))];
        
        userId = userId != null ? userId : message.SenderId;

        foreach (Message msg in previousMessages)
        {
            if (msg.SenderId == userId)
                previousMessagesText.Add(ChatMessage.CreateUserMessage(msg.Content));
            else
                previousMessagesText.Add(ChatMessage.CreateAssistantMessage(msg.Content));
        }

        var options = new ChatCompletionOptions
        {
            FrequencyPenalty = _openAISettings.Value.FrequencyPenalty,
            MaxOutputTokenCount = _openAISettings.Value.MaxOutputTokenCount,
            PresencePenalty = _openAISettings.Value.PresencePenalty,
            Temperature = _openAISettings.Value.Temperature,
            EndUserId = userId.ToString()
        };

        ChatCompletion response = await _chatClient.CompleteChatAsync(previousMessagesText, options);

        string responseText = response.Content[0].Text;
        Message responseMessage =
            new Message
            {
                Id = Guid.NewGuid(),
                Content = responseText,
                CreatedAt = DateTime.Now.ToUniversalTime(),
                ReceiverId = message.SenderId,
                SenderId = message.ReceiverId
            };

        return responseMessage;
    }
}
