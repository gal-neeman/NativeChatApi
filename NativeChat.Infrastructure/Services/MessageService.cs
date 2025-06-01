namespace NativeChat;

public class MessageService : IMessagesService
{
    private readonly IMessageDao _messageDao;
    private readonly IValidationService _validationService;

    public MessageService(IMessageDao messageDao, IValidationService validationService)
    {
        _messageDao = messageDao;
        _validationService = validationService;
    }

    public async Task<List<Message>> GetAllMessages(Guid botId, Guid userId)
    {
        return await _messageDao.GetAllMessages(botId, userId);
    }

    public async Task<Message?> SendMessageAsync(Guid userId, Message message)
    {
        if (userId != message.SenderId && userId != message.ReceiverId)
            return null;

        if (!(await _validationService.IsBotExistsAsync(message.SenderId) ||
            await _validationService.IsBotExistsAsync(message.ReceiverId)))
            return null;

        message.Id = Guid.NewGuid();

        await _messageDao.SendMessageAsync(message);

        return message;
    }
}
