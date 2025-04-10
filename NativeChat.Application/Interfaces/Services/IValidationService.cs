namespace NativeChat;
public interface IValidationService
{
    public Task<bool> IsEmailTaken(string email);

    public Task<bool> IsUserExistsAsync(CredentialsDto user);
}
