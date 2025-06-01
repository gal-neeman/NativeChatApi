using IMapper = AutoMapper.IMapper;

namespace NativeChat;

public class BotService : IBotService
{
    private readonly IValidationService _validationService;
    private readonly IBotDao _botDao;
    private readonly IMapper _mapper;

    public BotService(IValidationService validationService, IBotDao botDao, IMapper mapper)
    {
        _validationService = validationService;
        _botDao = botDao;
        _mapper = mapper;
    }

    public async Task<List<BotDto>?> GetUserBotsAsync(Guid userId)
    {
        if (!await _validationService.IsUserExistsAsync(userId))
            return null;

        var bots = await _botDao.GetBotsByUserIdAsync(userId);

        return bots.Select(bot => _mapper.Map<BotDto>(bot)).ToList();
    }

    public async Task<bool> DeleteBotAsync(Guid botId)
    {
        if (!await _validationService.IsBotExistsAsync(botId))
            return false;

        await _botDao.DeleteBotAsync(botId);
        return true;
    }

    public async Task<BotDto> AddBotAsync(NewBotDto botDto, Guid userId)
    {
        Bot bot = new Bot
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateOnly.FromDateTime(DateTime.Now),
            LanguageId = botDto.LanguageId,
            Name = botDto.Name,
            UserId = userId
        };

        await _botDao.AddBotAsync(bot);
        Bot? dbBot = await _botDao.GetBotByIdAsync(bot.Id);

        return _mapper.Map<BotDto>(dbBot);
    }
}
