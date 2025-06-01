namespace NativeChat;
public interface IValidationService
{
    public Task<bool> IsEmailTakenAsync(string email);

    public Task<bool> IsUserExistsAsync(CredentialsDto user);

    public Task<bool> IsUserExistsAsync(Guid userId);

    public Task<bool> IsBotExistsAsync(Guid botId);
}
