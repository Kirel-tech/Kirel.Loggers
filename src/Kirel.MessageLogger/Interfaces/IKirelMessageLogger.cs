using Microsoft.Extensions.Logging;

namespace Kirel.MessageLogger.Interfaces;

/// <summary>
/// Interface that represents type used for logging
/// </summary>
public interface IKirelMessageLogger : ILogger
{
    /// <summary>
    /// Logging at the information level
    /// </summary>
    /// <param name="logObj">Object that fields should be written to message</param>
    /// <param name="formatter">Function that writes object fields to a single string</param>
    /// <typeparam name="T">Object type</typeparam>
    public void LogInfo<T>(T logObj, Func<T, string> formatter);
    /// <summary>
    /// Logging at the debug level
    /// </summary>
    /// <param name="logObj">Object that fields should be written to message</param>
    /// <param name="formatter">Function that writes object fields to a single string</param>
    /// <typeparam name="T">Object type</typeparam>
    public void LogDebug<T>(T logObj, Func<T, string> formatter);
    /// <summary>
    /// Logging at the error level
    /// </summary>
    /// <param name="logObj">Object that fields should be written to message</param>
    /// <param name="ex">Exception to write to message</param>
    /// <param name="formatter">Function that writes object fields and exception message to a single string</param>
    /// <typeparam name="T">Object type</typeparam>
    public void LogError<T>(T logObj, Exception ex, Func<T, Exception, string> formatter);
    /// <summary>
    /// Logging at the critical level
    /// </summary>
    /// <param name="logObj">Object that fields should be written to message</param>
    /// <param name="ex">Exception to write to message</param>
    /// <param name="formatter">Function that writes object fields and exception message to a single string</param>
    /// <typeparam name="T">Object type</typeparam>
    public void LogCritical<T>(T logObj, Exception ex, Func<T, Exception, string> formatter);
}