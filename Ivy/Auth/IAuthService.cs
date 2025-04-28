namespace Ivy.Auth;

public interface IAuthService
{
    Task<string?> LoginAsync(string email, string password);
    
    Task LogoutAsync();
    
    Task<UserInfo?> GetUserInfoAsync();
}