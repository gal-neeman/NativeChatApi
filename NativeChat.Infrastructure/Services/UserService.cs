using AutoMapper;

namespace NativeChat;
public class UserService : IUserService
{
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IUserDao _userDao;
    private readonly IValidationService _validationService;

    public UserService(ITokenService tokenService, IMapper mapper, IUserDao userDao, IValidationService validationService)
    {
        _mapper = mapper;
        _tokenService = tokenService;
        _userDao = userDao;
        _validationService = validationService;
    }

    public async Task<string?> RegisterAsync(RegisterDto registerDto)
    {
        if (await _validationService.IsEmailTaken(registerDto.Email.ToLower()))
            return null;

        var user = _mapper.Map<User>(registerDto);
        user.Id = Guid.NewGuid();
        user.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
        user.Password = Encryptor.GetHashed(registerDto.Password);
        user.Email = registerDto.Email.ToLower();

        await _userDao.AddUserAsync(user);

        return _tokenService.GetNewToken(user);
    }

    public async Task<string?> LoginAsync(CredentialsDto credentials)
    {
        if (!await _validationService.IsUserExistsAsync(credentials))
            return null;

        var user = await _userDao.GetUserAsync(credentials);

        return _tokenService.GetNewToken(user);
    }
}
