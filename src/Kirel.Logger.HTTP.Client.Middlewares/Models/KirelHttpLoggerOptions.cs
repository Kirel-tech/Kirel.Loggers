﻿namespace Kirel.Logger.HTTP.Client.Middlewares.Models;

/// <summary>
/// Application options for kirel db logger
/// </summary>
public class KirelHttpLoggerOptions
{
    /// <summary>
    /// Authorization header for current application
    /// </summary>
    public string AuthorizationHeader { get; set; }
    /// <summary>
    /// Flag that represents if http logging enabled
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// The name of the application to to associate logs in db with it
    /// </summary>
    public string Source { get; set; } = "";
    /// <summary>
    /// The uri that will be used to send logs to be written to the database
    /// </summary>
    public string Uri { get; set; } = "";
}