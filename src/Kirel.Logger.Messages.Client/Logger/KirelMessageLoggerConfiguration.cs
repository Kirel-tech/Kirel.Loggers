using Microsoft.Extensions.Logging;

namespace Kirel.Logger.Messages.Client.Logger;

public class KirelMessageLoggerConfiguration
{
    public string ApiKey { get; set; } = "";
    public string AuthorizationHeader { get; set; } = "";
    public string Uri { get; set; } = new("https://localhost");
    public Dictionary<string, LogLevel> KirelLogLevel { get; set; } = new()
    {
        ["Default"] = LogLevel.Information
    };
}