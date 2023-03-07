using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kirel.Logger.Shared.Handlers;

/// <summary>
/// Handler class which authenticate request with API key
/// </summary>
public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private IConfiguration _configuration;

    /// <summary>
    /// Returns instance of API key handler
    /// </summary>
    /// <param name="options">Contains authentication options.</param>
    /// <param name="logger">Represents a type used to configure the logging system and create instances of ILogger.</param>
    /// <param name="encoder">Represents a URL character encoding.</param>
    /// <param name="clock">Abstracts the system clock to facilitate testing.</param>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Handles api key authentication
    /// </summary>
    /// <returns>Asynchronous operation that contains the result of an Authenticate call</returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var apiKeyHeader = "X-API-KEY";
        if (!Request.Headers.ContainsKey(apiKeyHeader))
            return await Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        var apiKey = Request.Headers[apiKeyHeader];

        if (string.IsNullOrEmpty(apiKey))
        {
            return await Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        }

        try
        {
            return ValidateApiKey(apiKey);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(AuthenticateResult.Fail(ex.Message));
        }
    }

    private AuthenticateResult ValidateApiKey(string apiKey)
    {
        var apiKeys = _configuration.GetSection("APIKeys").GetChildren().ToArray().Select(c => c.Value).ToArray();

        if (!apiKeys.Contains(apiKey))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var identity = GetClaimsIdentity();
        var principal = new System.Security.Principal.GenericPrincipal(identity, new[] { "Microservice" });
        return AuthenticateResult.Success(new AuthenticationTicket(principal, "APIKey"));
    }

    private ClaimsIdentity GetClaimsIdentity()
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, "Microservice"),
        };

        var claimsIdentity =
            new ClaimsIdentity(claims, "APIKey", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}