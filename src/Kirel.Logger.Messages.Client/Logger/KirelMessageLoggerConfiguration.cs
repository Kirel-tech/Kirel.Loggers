using Microsoft.Extensions.Logging;

namespace Kirel.Logger.Messages.Client.Logger;

/// <summary>
/// Kirel logger configuration class
/// </summary>
public class KirelMessageLoggerConfiguration
{
    /// <summary>
    /// API key value for authorize in message logger API 
    /// </summary>
    public string ApiKey { get; set; } = "";
    /// <summary>
    /// Authorization header for authorize in message logger API 
    /// </summary>
    public string AuthorizationHeader { get; set; } = "";
    /// <summary>
    /// Message logger API uri
    /// </summary>
    public string Uri { get; set; } = new("https://localhost");
    /// <summary>
    /// Minimal log level
    /// </summary>
    public Dictionary<string, LogLevel> KirelLogLevel { get; set; } = new()
    {
        ["Default"] = LogLevel.Information
    };
}