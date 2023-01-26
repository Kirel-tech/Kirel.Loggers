using Kirel.Repositories.Interfaces;

namespace Kirel.MessageLogger.API.Models;

/// <summary>
/// Database log entity
/// </summary>
public class KirelLogMessage : IKeyEntity<Guid>, ICreatedAtTrackedEntity
{
    /// <summary>
    /// Log unique identifier
    /// </summary>
    public Guid Id { get; set; }      
    /// <summary>
    /// Log create date and time
    /// </summary>
    public DateTime Created { get; set; }
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
    /// Source application that sent this dto
    /// </summary>
    public string Source { get; set; }
    /// <summary>
    /// Message severity level
    /// </summary>
    public string Level { get; set; }
}