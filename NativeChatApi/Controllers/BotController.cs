using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace NativeChat;

[Route("api/v1/[controller]")]
[ApiController]
public class BotsController : ControllerBase
{
    private readonly IBotService _botService;

    public BotsController(IBotService botService)
    {
        _botService = botService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserBotsAsync()
    {
        string? userId = HttpContext.Items["UserId"] as string;
        if (userId == null)
            return Unauthorized();
        Guid guid = Guid.Parse(userId);

        var bots = await _botService.GetUserBotsAsync(guid);

        if (bots == null)
            return NotFound("User does not exist");

        return Ok(bots);
    }

    [HttpDelete("{botId}")]
    public async Task<IActionResult> DeleteBotAsync([FromRoute] Guid botId)
    {
        if (!(await _botService.DeleteBotAsync(botId)))
            return NotFound("This bot does not exist.");

        return NoContent();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> AddBotAsync([FromBody] NewBotDto botDto)
    {
        string? userId = HttpContext.Items["UserId"] as string;
        if (userId == null)
            return Unauthorized();
        Guid guid = Guid.Parse(userId);

        BotDto dbBotDto = await _botService.AddBotAsync(botDto, guid);

        return Created("/", dbBotDto);
    }
}
