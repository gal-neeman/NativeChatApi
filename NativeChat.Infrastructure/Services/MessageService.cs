namespace NativeChat;

public class MessageService : IMessagesService
{
    private readonly IMessageDao _messageDao;
    private readonly IValidationService _validationService;
    private readonly IChatService _chatService;

    public MessageService(IMessageDao messageDao, IValidationService validationService, IChatService chatService)
    {
        _messageDao = messageDao;
        _validationService = validationService;
        _chatService = chatService;
    }

    public async Task<List<Message>> GetAllMessages(Guid botId, Guid userId)
    {
        return await _messageDao.GetAllMessages(botId, userId);
    }

    public async Task<MessageDto?> SendMessageAsync(Guid userId, Message message)
    {
        if (userId != message.SenderId && userId != message.ReceiverId)
            return null;

        if (!(await _validationService.IsBotExistsAsync(message.SenderId) ||
            await _validationService.IsBotExistsAsync(message.ReceiverId)))
            return null;

        message.Id = Guid.NewGuid();

        Message responseMessage = await _chatService.CompleteChatAsync(message);

        await _messageDao.SendMessagesAsync([message, responseMessage]);

        MessageDto messageDto = new MessageDto
        {
            receivedMessage = message,
            responseMessage = responseMessage
        };

        return messageDto;
    }
}
