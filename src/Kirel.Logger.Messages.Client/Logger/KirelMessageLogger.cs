using System.Text;
using System.Net.Http;
using System.Reflection;
using Kirel.Logger.Messages.Client.Interfaces;
using Kirel.Logger.Messages.DTOs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kirel.Logger.Messages.Client.Logger;

/// <summary>
/// Class that implements <see cref="IKirelMessageLogger"/> with writing logs to database by API
/// </summary>
public class KirelMessageLogger : IKirelMessageLogger
{
    private readonly string _name;
    private readonly Func<KirelMessageLoggerConfiguration> _getCurrentConfig;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Returns an instance of the database logger
    /// </summary>
    /// <param name="loggerOpt">Logger options</param>
    /// <param name="config">Application configuration</param>
    /// <param name="httpClient">Instance of the http client</param>
    public KirelMessageLogger(string name,
        Func<KirelMessageLoggerConfiguration> getCurrentConfig, HttpClient httpClient)
    {
        _httpClient = httpClient;
        (_name, _getCurrentConfig) = (name, getCurrentConfig);
    }
    /// <summary>
    /// Function that creates log dto and send it to database by API
    /// </summary>
    /// <param name="logLevel">Current log level</param>
    /// <param name="eventId">Event Id</param>
    /// <param name="state">Object that fields should be written to message</param>
    /// <param name="exception">Exception to write to message</param>
    /// <param name="formatter">Function that writes object fields and exception message to a single string</param>
    /// <typeparam name="TState">Object type</typeparam>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;
        var config = _getCurrentConfig(); 
        var uri = new Uri(config.Uri);
        var log = new KirelLogMessageCreateDto
        {
            Message = formatter(state, exception),
            ExceptionMessage = exception?.Message,
            InnerExceptionMessage = exception?.InnerException?.Message,
            StackTrace = exception?.StackTrace,
            Service = Assembly.GetEntryAssembly()?.GetName().Name,
            Source = _name,
            Level = logLevel.ToString()
        };

        var json = JsonConvert.SerializeObject(log);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _httpClient.PostAsync(uri, content);
    }
    /// <summary>
    /// Checks if the log level is enabled
    /// </summary>
    /// <param name="logLevel">Log level which enable status would be checked</param>
    /// <returns>True if enabled, otherwise false</returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _getCurrentConfig().KirelLogLevel["Default"];
    }
    
    /// <summary>
    /// Begins a logical operation scope
    /// </summary>
    /// <param name="state">The identifier for the scope </param>
    /// <returns>An <see cref="T:System.IDisposable" /> that ends the logical operation scope on dispose.</returns>
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;

    /// <summary>
    /// Logging at the information level
    /// </summary>
    /// <param name="logObj">Object that fields should be written to message</param>
    /// <param name="formatter">Function that writes object fields to a single string</param>
    /// <typeparam name="T">Object type</typeparam>
    public void LogInfo<T>(T logObj,  Func<T, string> formatter)
    {
        Log(LogLevel.Information, new EventId(0), logObj, null, (obj, _) => formatter(obj));
    }
    /// <summary>
    /// Logging at the debug level
    /// </summary>
    /// <param name="logObj">Object that fields should be written to message</param>
    /// <param name="formatter">Function that writes object fields to a single string</param>
    /// <typeparam name="T">Object type</typeparam>
    public void LogDebug<T>(T logObj,  Func<T, string> formatter)
    {
        Log(LogLevel.Debug, new EventId(0), logObj, null, (obj, _) => formatter(obj));
    }
    /// <summary>
    /// Logging at the error level
    /// </summary>
    /// <param name="logObj">Object that fields should be written to message</param>
    /// <param name="ex">Exception to write to message</param>
    /// <param name="formatter">Function that writes object fields and exception message to a single string</param>
    /// <typeparam name="T">Object type</typeparam>
    public void LogError<T>(T logObj, Exception ex,  Func<T, Exception, string> formatter)
    {
        Log(LogLevel.Error, new EventId(0), logObj, ex, formatter);
    }
    /// <summary>
    /// Logging at the critical level
    /// </summary>
    /// <param name="logObj">Object that fields should be written to message</param>
    /// <param name="ex">Exception to write to message</param>
    /// <param name="formatter">Function that writes object fields and exception message to a single string</param>
    /// <typeparam name="T">Object type</typeparam>
    public void LogCritical<T>(T logObj, Exception ex,  Func<T, Exception, string> formatter)
    {
        Log(LogLevel.Critical, new EventId(0), logObj, ex, formatter);
    }
}