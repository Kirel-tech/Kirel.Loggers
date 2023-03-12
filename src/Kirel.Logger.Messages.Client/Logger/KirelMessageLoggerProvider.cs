using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kirel.Logger.Messages.Client.Logger;

/// <inheritdoc />
[ProviderAlias("KirelMessageLogger")]
public class KirelMessageLoggerProvider : ILoggerProvider
{
    private readonly IDisposable _onChangeToken;
    private KirelMessageLoggerConfiguration _currentConfig;
    private HttpClient _httpClient;
    private readonly ConcurrentDictionary<string, KirelMessageLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerOpt"></param>
    /// <param name="config"></param>
    /// <param name="httpClient"></param>
    public KirelMessageLoggerProvider(IOptionsMonitor<KirelMessageLoggerConfiguration> loggerOpt)
    {
        _currentConfig = loggerOpt.CurrentValue;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_currentConfig.Uri);
        if (_currentConfig.AuthorizationHeader == "X-API-KEY")
        {
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _currentConfig.ApiKey);
        }
        _onChangeToken = loggerOpt.OnChange(updatedConfig => _currentConfig = updatedConfig);
    }
    
    private KirelMessageLoggerConfiguration GetCurrentConfig() => _currentConfig;

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new KirelMessageLogger(name, GetCurrentConfig, _httpClient));


    /// <inheritdoc />
    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }
}