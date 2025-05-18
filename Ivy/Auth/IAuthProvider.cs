namespace Ivy.Auth;

public interface IAuthProvider
{
     Task<string?> LoginAsync(string email, string password);

     Task LogoutAsync(string jwt);

     Task<bool> ValidateJwtAsync(string jwt);

     Task<UserInfo?> GetUserInfoAsync(string jwt);

     AuthOption[] GetAuthOptions();
}