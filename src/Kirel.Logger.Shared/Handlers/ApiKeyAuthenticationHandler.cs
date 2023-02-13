using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kirel.Logger.Shared.Handlers;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private IConfiguration _configuration;
    public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var apiKeyHeader = "Authorization";
        if (!Request.Headers.ContainsKey(apiKeyHeader))
            return await Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        string authorizationHeader = Request.Headers[apiKeyHeader];
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return await Task.FromResult(AuthenticateResult.NoResult());
        }

        var apikeyArr = authorizationHeader.Split(" ");
        if(apikeyArr.Length < 2) return await Task.FromResult(AuthenticateResult.NoResult());
        var apikey = apikeyArr[1];
        
        if (string.IsNullOrEmpty(apikey))
        {
            return await Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        }
 
        try
        {
            return ValidateApiKey(apikey);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(AuthenticateResult.Fail(ex.Message));
        }
    }

    private AuthenticateResult ValidateApiKey(string transferedApiKey)
    {
        var apiKeys = _configuration.GetSection("ApiKeys").GetChildren().ToArray().Select(c => c.Value).ToArray();
        
        if (!apiKeys.Contains(transferedApiKey))
        {
            return AuthenticateResult.Fail("Unauthorized"); 
        }
        var identity = GetClaimsIdentity();
        var principal = new System.Security.Principal.GenericPrincipal(identity, new []{"LoggerWriter"});
        return AuthenticateResult.Success(new AuthenticationTicket(principal, "APIKey"));
    }
    
    private ClaimsIdentity GetClaimsIdentity()
    {
        var claims = new List<Claim>
        {
            new (ClaimsIdentity.DefaultNameClaimType, "LoggerWriter"),
        };
        
        var claimsIdentity =
            new ClaimsIdentity(claims, "APIKey", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }

}