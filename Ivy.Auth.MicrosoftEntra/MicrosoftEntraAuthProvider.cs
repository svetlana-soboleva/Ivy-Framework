using System.Reflection;
using Ivy.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Ivy.Hooks;

namespace Ivy.Auth.MicrosoftEntra;

public class MicrosoftEntraOAuthException(string? error, string? errorCode, string? errorDescription)
    : Exception($"Microsoft Entra error: '{error}', code '{errorCode}' - {errorDescription}")
{
    public string? Error { get; } = error;
    public string? ErrorCode { get; } = errorCode;
    public string? ErrorDescription { get; } = errorDescription;
}

public class MicrosoftEntraAuthProvider : IAuthProvider
{
    private readonly IConfidentialClientApplication _app;
    private readonly string[] _scopes = ["User.Read", "openid", "profile", "email"];

    private readonly List<AuthOption> _authOptions = [];

    private string? _codeVerifier = null;

    public MicrosoftEntraAuthProvider()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets(Assembly.GetEntryAssembly()!)
            .Build();

        var tenantId = configuration.GetValue<string>("MICROSOFT_ENTRA_TENANT_ID") ?? throw new Exception("MICROSOFT_ENTRA_TENANT_ID is required");
        var clientId = configuration.GetValue<string>("MICROSOFT_ENTRA_CLIENT_ID") ?? throw new Exception("MICROSOFT_ENTRA_CLIENT_ID is required");
        var clientSecret = configuration.GetValue<string>("MICROSOFT_ENTRA_CLIENT_SECRET") ?? throw new Exception("MICROSOFT_ENTRA_CLIENT_SECRET is required");

        // Create a confidential client application for OAuth flow
        _app = ConfidentialClientApplicationBuilder
            .Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
            // TODO: don't hardcode this
            .WithRedirectUri("http://localhost:5010/webhook")
            .Build();
    }

    public Task<AuthToken?> LoginAsync(string email, string password)
        => throw new InvalidOperationException("Microsoft Entra login with email/password is not supported");

    public async Task<Uri> GetOAuthUriAsync(AuthOption option, WebhookEndpoint callback)
    {
        _codeVerifier = GenerateCodeVerifier();
        var codeChallenge = GenerateCodeChallenge(_codeVerifier);

        var authUrl = await _app
            .GetAuthorizationRequestUrl(_scopes)
            .WithRedirectUri(callback.GetUri(includeIdInPath: false).ToString())
            .WithExtraQueryParameters(new Dictionary<string, string>
            {
                ["code_challenge"] = codeChallenge,
                ["code_challenge_method"] = "S256",
                ["state"] = callback.Id,
            })
            .ExecuteAsync();

        return authUrl;
    }

    public async Task<AuthToken?> HandleOAuthCallbackAsync(HttpRequest request)
    {
        var code = request.Query["code"].ToString();
        var error = request.Query["error"].ToString();
        var errorDescription = request.Query["error_description"].ToString();

        if (error.Length > 0 || errorDescription.Length > 0)
        {
            throw new MicrosoftEntraOAuthException(error, null, errorDescription);
        }
        else if (code.Length == 0)
        {
            throw new Exception("Received no authorization code from Microsoft Entra.");
        }

        if (_codeVerifier == null)
        {
            throw new Exception("Code verifier not found. OAuth flow was not properly initiated.");
        }

        var result = await _app.AcquireTokenByAuthorizationCode(_scopes, code)
            .WithPkceCodeVerifier(_codeVerifier)
            .ExecuteAsync();

        var authToken = new AuthToken(
            result.AccessToken,
            result.Account?.HomeAccountId?.Identifier,
            result.ExpiresOn
        );

        return authToken;
    }

    public async Task LogoutAsync(string jwt)
    {
        // For Microsoft Entra, logout typically involves clearing the token cache
        // The actual logout URL would be handled by the client application
        await _app.RemoveAsync(null);
    }

    public async Task<AuthToken?> RefreshJwtAsync(AuthToken jwt)
    {
        if (jwt.ExpiresAt == null || DateTimeOffset.UtcNow < jwt.ExpiresAt)
        {
            // Refresh not needed (or not possible).
            return jwt;
        }

        if (string.IsNullOrEmpty(jwt.RefreshToken))
        {
            // Can't refresh without account identifier
            return null;
        }

        try
        {
            // Get all accounts from token cache
            IEnumerable<IAccount> accounts;
            try
            {
                // Use GetAccountsAsync but we'll handle finding the right account ourselves
                accounts = await _app.GetAccountsAsync();
            }
            catch (MsalException)
            {
                // Fallback if GetAccountsAsync fails
                return null;
            }

            // Try to find the account using the RefreshToken (which stores the account identifier)
            var account = accounts.FirstOrDefault(a => a.HomeAccountId.Identifier == jwt.RefreshToken);

            if (account == null)
            {
                // Account not found in token cache
                return null;
            }

            // Acquire token silently using the account
            var result = await _app.AcquireTokenSilent(_scopes, account)
                .ExecuteAsync();

            return new AuthToken(
                result.AccessToken,
                result.Account?.HomeAccountId?.Identifier,
                result.ExpiresOn
            );
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> ValidateJwtAsync(string jwt)
    {
        try
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

            var tempGraphClient = new GraphServiceClient(httpClient);
            var user = await tempGraphClient.Me.GetAsync();
            return user != null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<UserInfo?> GetUserInfoAsync(string jwt)
    {
        try
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

            var tempGraphClient = new GraphServiceClient(httpClient);
            var user = await tempGraphClient.Me.GetAsync();

            if (user == null || user.Id == null)
            {
                return null;
            }

            return new UserInfo(
                user.Id,
                user.Mail ?? user.UserPrincipalName ?? string.Empty,
                user.DisplayName,
                null // Microsoft Graph doesn't provide avatar URL in basic user info
            );
        }
        catch (Exception)
        {
            return null;
        }
    }

    public AuthOption[] GetAuthOptions()
    {
        return [.. _authOptions];
    }

    public MicrosoftEntraAuthProvider UseMicrosoftEntra()
    {
        _authOptions.Add(new AuthOption(AuthFlow.OAuth, "Microsoft", "microsoft", Icons.Microsoft));
        return this;
    }

    private static string GenerateCodeVerifier()
    {
        var bytes = new byte[32];
        System.Security.Cryptography.RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static string GenerateCodeChallenge(string codeVerifier)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(codeVerifier);
        var hash = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToBase64String(hash)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

}