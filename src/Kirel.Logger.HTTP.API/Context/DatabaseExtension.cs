using Kirel.Logger.HTTP.API.Models;
using Kirel.Logger.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Kirel.Logger.HTTP.API.Context;

/// <summary>
/// Extension for service collection that adds data db context depends on chosen db driver in config file
/// </summary>
public static class DatabaseExtension
{
    /// <summary>
    /// Adds db context depends on chosen db driver in config file
    /// </summary>
    /// <param name="services">This service collection</param>
    /// <param name="dbConfig">Database config</param>
    /// <exception cref="Exception">Throws if db driver unsupported</exception>
    public static void AddApplicationContext(this IServiceCollection services, DbConfig dbConfig)
    {
        Action<DbContextOptionsBuilder> contextOptions = dbConfig.Driver switch
        {
            "mssql" => builder => builder.UseSqlServer(GetMsSqlConnectionString(dbConfig)),
            "postgresql" => builder => builder.UseNpgsql(GetPostgreSqlConnectionString(dbConfig)),
            "mysql" => builder =>
            {
                var connectionString = GetMySqlConnectionString(dbConfig);
                var version = ServerVersion.AutoDetect(connectionString);
                builder.UseMySql(connectionString, version);
                
            },
            _ => throw new Exception("Unsupported database driver. Use one of this mssql/postgresql/mysql")
        };
    
        switch (dbConfig.Driver)
        {
            case "mysql":
                services.AddDbContext<KirelLogHttpDbContext, KirelLogHttpMysqlDbContext>(contextOptions);
                break;
            case "postgresql":
                services.AddDbContext<KirelLogHttpDbContext, KirelLogHttpPostgresqlDbContext>(contextOptions);
                break;
            case "mssql":
                services.AddDbContext<KirelLogHttpDbContext, KirelLogHttpMssqlDbContext>(contextOptions);
                break;
        }
    }

    private static string GetMySqlConnectionString(DbConfig dbConfig)
    {
        return $"Server={dbConfig.Config.Address};" +
               $"Port={dbConfig.Config.Port};" +
               $"Database={dbConfig.Config.DatabaseName};" +
               $"Uid={dbConfig.Config.User};" +
               $"Pwd={dbConfig.Config.Password};";
    }

    private static string GetMsSqlConnectionString(DbConfig dbConfig)
    {
        return $"Server={dbConfig.Config.Address},{dbConfig.Config.Port};" +
               $"Database={dbConfig.Config.DatabaseName};" +
               $"User Id={dbConfig.Config.User};" +
               $"Password={dbConfig.Config.Password};";
    }

    private static string GetPostgreSqlConnectionString(DbConfig dbConfig)
    {
        return $"Server={dbConfig.Config.Address};" +
               $"Port={dbConfig.Config.Port};" +
               $"Database={dbConfig.Config.DatabaseName};" +
               $"User Id={dbConfig.Config.User};" +
               $"Password={dbConfig.Config.Password};";
    }
}
