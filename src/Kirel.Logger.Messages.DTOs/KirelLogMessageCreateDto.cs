namespace Kirel.Logger.Messages.DTOs;

/// <summary>
/// Data transfer object for create log
/// </summary>
public class KirelLogMessageCreateDto
{
    /// <summary>
    /// Log message
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// Exception message
    /// </summary>
    public string ExceptionMessage { get; set; }
    /// <summary>
    /// Inner exception message
    /// </summary>
    public string InnerExceptionMessage { get; set; }
    /// <summary>
    /// Stack trace
    /// </summary>
    public string StackTrace { get; set; }
    /// <summary>
    /// The name of the service that sent this log
    /// </summary>
    public string Service { get; set; }
    /// <summary>
    /// Source application that sent this dto
    /// </summary>
    public string Source { get; set; }
    /// <summary>
    /// Message severity level
    /// </summary>
    public string Level { get; set; }
}