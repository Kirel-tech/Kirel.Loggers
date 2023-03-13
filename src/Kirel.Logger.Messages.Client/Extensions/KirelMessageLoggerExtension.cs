using Kirel.Logger.Messages.Client.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Kirel.Logger.Messages.Client.Extensions;

/// <summary>
/// Extension class for adding <see cref="KirelMessageLogger"/> as .NET logger
/// </summary>
public static class KirelMessageLoggerExtension
{
    /// <summary>
    /// Register <see cref="KirelMessageLogger"/> as a .NET logger.
    /// Logger gets settings from appsetting.json. Make sure the following fields are in the 'Logging' section
    /// <code>
    /// "KirelMessageLogger": {
    ///     "LogLevel": {
    ///         "Default": "Information"
    ///  },
    /// "Uri": "https://localhost/api/log/message",
    /// "AuthorizationHeader": "X-API-KEY",
    /// "ApiKey": "ExampleAPIKey123!"
    /// }
    /// </code>
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/>.</param>
    public static ILoggingBuilder AddKirelMessageLogger(
        this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, KirelMessageLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions
            <KirelMessageLoggerConfiguration, KirelMessageLoggerProvider>(builder.Services);

        return builder;
    }

    /// <summary>
    /// Register <see cref="KirelMessageLogger"/> as a .NET logger.
    /// All settings fields can be rewritten with 'configure'
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/>.</param>
    /// <param name="configure">Sets logger settings fields above appsettings.json.</param>
    /// <returns></returns>
    public static ILoggingBuilder AddKirelMessageLogger(
        this ILoggingBuilder builder,
        Action<KirelMessageLoggerConfiguration> configure)
    {
        builder.AddKirelMessageLogger();
        builder.Services.Configure(configure);

        return builder;
    }
}