using Microsoft.AspNetCore.Mvc;

namespace NativeChat;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        string? token = await _userService.RegisterAsync(registerDto);

        if (token == null)
            return BadRequest("Email is already taken.");

        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(CredentialsDto credentialsDto)
    {
        string? token = await _userService.LoginAsync(credentialsDto);

        if (token == null)
            return BadRequest("Incorrect email or password");

        return Ok(token);
    }
}
