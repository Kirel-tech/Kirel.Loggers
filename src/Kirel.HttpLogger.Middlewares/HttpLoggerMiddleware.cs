using System.Text;
using Kirel.HttpLogger.DTOs;
using Kirel.HttpLogger.Middlewares.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Kirel.HttpLogger.Middlewares;

/// <summary>
/// Middleware that logs both http request and response
/// </summary>
public class KirelHttpLoggerMiddleware
{ 
    private readonly RequestDelegate _next;
    private readonly HttpClient _httpClient;
    private readonly IOptions<KirelHttpLoggerOptions> _loggerOpt;

    /// <summary>
    /// Return instance of a http logger middleware
    /// </summary>
    /// <param name="next">A function that can process an HTTP request</param>
    /// <param name="httpClient">Http client class for sending http requests</param>
    /// <param name="loggerOpt">Kirel logger options</param>
    public KirelHttpLoggerMiddleware(RequestDelegate next, HttpClient httpClient, IOptions<KirelHttpLoggerOptions> loggerOpt)
    {
        _next = next;
        _httpClient = httpClient;
        _loggerOpt = loggerOpt;
    }
    /// <summary>
    /// Middleware call point
    /// </summary>
    /// <param name="context">Http context that encapsulates all HTTP-specific information about an individual HTTP request</param>
    public async Task Invoke(HttpContext context)
    {
        var httpLog = new KirelLogHttpDto();
        ParseRequest(httpLog, context);
        await ParseResponse(httpLog, context);
        try
        {
            SendLog(httpLog);
        }
        catch
        {
            // ignored
        }
    }

    private void SendLog(KirelLogHttpDto httpLog)
    {
        var json = JsonConvert.SerializeObject(httpLog);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _httpClient.PostAsync(_loggerOpt.Value.Uri, content);
    }

    private async Task ParseResponse(KirelLogHttpDto httpLog, HttpContext context)
    {
        //Save original response body stream, for restoring at the end
        var originalResponseBody = context.Response.Body;
        // Substitute response body stream in context
        await using var newResponseBody = new MemoryStream();
        context.Response.Body = newResponseBody;
        // Handle request
        await _next(context);
        // Write httplog fields
        httpLog.ResponseHeaders = FormatHeaders(context.Response.Headers);
        httpLog.ResponseBody = await ReadHttpBody(context.Response.Body);
        httpLog.ResponseCode = context.Response.StatusCode;
        //Copy new response stream to original and restore original body to response
        await newResponseBody.CopyToAsync(originalResponseBody);
        context.Response.Body = originalResponseBody;
        await newResponseBody.DisposeAsync();
    }

    private async void ParseRequest(KirelLogHttpDto httpLog, HttpContext context)
    {
        context.Request.EnableBuffering();
        httpLog.Username = context.User.Identity?.Name;
        httpLog.Source = _loggerOpt.Value.Source;
        httpLog.Host = context.Request.Host.ToString();
        httpLog.Path = context.Request.Path;
        httpLog.Method = context.Request.Method;
        httpLog.Protocol = context.Request.Protocol;
        httpLog.ClientIp = context.Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        httpLog.QueryString = context.Request.QueryString.ToString();
        httpLog.RequestId = context.TraceIdentifier;
        httpLog.RequestHeaders = FormatHeaders(context.Request.Headers);
        httpLog.RequestBody = await ReadHttpBody(context.Request.Body);
    }
    
    private static string FormatHeaders(IHeaderDictionary headers) => 
        string.Join(", ", headers
            .Where(kvp => !kvp.Key.Contains("Authorization", StringComparison.OrdinalIgnoreCase))
            .Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));
    private async Task<string> ReadHttpBody(Stream body)
    {
        body.Seek(0, SeekOrigin.Begin);
        var stringBody = await new StreamReader(body).ReadToEndAsync();
        body.Seek(0, SeekOrigin.Begin);
    
        return stringBody;
    }
}