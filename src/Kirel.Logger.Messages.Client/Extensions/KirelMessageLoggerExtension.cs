using Kirel.Logger.Messages.Client.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Kirel.Logger.Messages.Client.Extensions;

public static class KirelMessageLoggerExtension
{
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

    public static ILoggingBuilder AddKirelMessageLogger(
        this ILoggingBuilder builder,
        Action<KirelMessageLoggerConfiguration> configure)
    {
        builder.AddKirelMessageLogger();
        builder.Services.Configure(configure);

        return builder;
    }
}