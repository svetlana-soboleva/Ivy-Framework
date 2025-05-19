using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ivy.Auth;

public interface IAuthService
{
    Task<string?> LoginAsync(string email, string password);

    Task<Uri> GetOAuthUriAsync(string optionId, Uri callbackUri);
    
    string HandleOAuthCallback(HttpRequest request);
    
    Task LogoutAsync();
    
    Task<UserInfo?> GetUserInfoAsync();
    
    AuthOption[] GetAuthOptions();
}