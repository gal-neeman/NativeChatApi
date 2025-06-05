using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace NativeChat;

public class UserCheckupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOptions<UserCheckupSettings> _checkupSettings;

    public UserCheckupService(IServiceScopeFactory scopeFactory, IOptions<UserCheckupSettings> checkupSettings)
    {
        _scopeFactory = scopeFactory;
        _checkupSettings = checkupSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Set clean-up interval
        TimeSpan checkupInterval = new TimeSpan(_checkupSettings.Value.CheckupInterval, 0, 0, 0);
        using var scopeFactory = _scopeFactory.CreateScope();
        IUserDao _userDao = scopeFactory.ServiceProvider.GetRequiredService<IUserDao>();
        IMessageDao _messageDao = scopeFactory.ServiceProvider.GetRequiredService<IMessageDao>();
        IChatService _chatService = scopeFactory.ServiceProvider.GetRequiredService<IChatService>();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                ICollection<User> users = await _userDao.GetAllUsersAsync();

                foreach (var user in users)
                {
                    foreach (var bot in user.Bots)
                    {
                        Message? lastMessage = await _messageDao.GetLastMessageAsync(user.Id, bot.Id);
                        if (lastMessage != null && lastMessage.SenderId != bot.Id)
                        {
                            TimeSpan time = DateTime.Now - lastMessage.CreatedAt;
                            if (time.TotalMinutes > Random.Shared.Next(_checkupSettings.Value.MaxRandomMinutes))
                            {
                                Message checkupMessage = new Message
                                {
                                    Content = _checkupSettings.Value.CheckupMessage,
                                    CreatedAt = DateTime.Now,
                                    Id = Guid.NewGuid(),
                                    ReceiverId = bot.Id,
                                    SenderId = user.Id
                                };

                                Log.Information("User Checkup Service: Sending a check up message to user: " + user.Id);
                                Message response = await _chatService.CompleteChatAsync(checkupMessage);
                                await _messageDao.SendMessageAsync(response);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("An error has occured while trying to remove a log file:\n" + e.Message);
            }

            // Wait until next cleanup date
            await Task.Delay(checkupInterval);
        }
    }
}
