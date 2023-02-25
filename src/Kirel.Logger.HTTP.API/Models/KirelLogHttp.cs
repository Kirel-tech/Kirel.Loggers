using Kirel.Repositories.Interfaces;

namespace Kirel.Logger.HTTP.API.Models;

/// <summary>
/// Http database log entity
/// </summary>
public class KirelLogHttp : IKeyEntity<Guid>, ICreatedAtTrackedEntity
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
    /// Unique name of the user
    /// </summary>
    public string Username { get; set; }
    /// <summary>
    /// Source application that sent this dto
    /// </summary>
    public string Source { get; set; }
    /// <summary>
    /// Host that accept this dto
    /// </summary>
    public string Host { get; set; }
    /// <summary>
    /// Http path
    /// </summary>
    public string Path { get; set; }
    /// <summary>
    /// Http method
    /// </summary>
    public string Method { get; set; }
    /// <summary>
    /// Http protocol
    /// </summary>
    public string Protocol { get; set; }
    /// <summary>
    /// Sender client Ip
    /// </summary>
    public string ClientIp { get; set; }
    /// <summary>
    /// Request query string
    /// </summary>
    public string QueryString { get; set; }    
    /// <summary>
    /// Request trace identifier
    /// </summary>
    public string RequestId { get; set; }      
    /// <summary>
    /// Request headers
    /// </summary>
    public string RequestHeaders { get; set; } 
    /// <summary>
    /// Request body
    /// </summary>
    public string RequestBody { get; set; }
    /// <summary>
    /// Response headers
    /// </summary>
    public string ResponseHeaders { get; set; }
    /// <summary>
    /// Response body
    /// </summary>
    public string ResponseBody { get; set; }
    /// <summary>
    /// Response code
    /// </summary>
    public int ResponseCode { get; set; }
}