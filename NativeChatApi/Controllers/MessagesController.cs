using Microsoft.AspNetCore.Mvc;

namespace NativeChat;

[Route("api/v1/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly IMessagesService _messageService;

    public MessagesController(IMessagesService messagesService)
    {
        _messageService = messagesService;
    }

    [HttpGet("{botId}")]
    public async Task<IActionResult> GetChatMessagesAsync([FromRoute] Guid botId)
    {
        string? userId = HttpContext.Items["UserId"] as string;
        if (userId == null)
            return Unauthorized();
        Guid guid = Guid.Parse(userId);

        List<Message> messages = await _messageService.GetAllMessages(botId, guid);

        return Ok(messages);
    }
}
