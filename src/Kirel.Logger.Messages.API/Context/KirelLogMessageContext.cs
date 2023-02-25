using Kirel.Logger.Messages.API.Models;
using Kirel.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kirel.Logger.Messages.API.Context;

/// <summary>
/// Derived class from <see cref="DbContext"/> for working with logs
/// </summary>
public class KirelLogMessageContext : DbContext
{
    /// <summary>
    /// Logs query
    /// </summary>
    public DbSet<KirelLogMessage> Logs => Set<KirelLogMessage>();

    /// <summary>
    /// Returns an instance of the database context
    /// </summary>
    /// <param name="dbContextOptions">The options for this context</param>
    public KirelLogMessageContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        Database.EnsureCreated();
    } 

    /// <summary>
    /// Method which called when entity saved
    /// </summary>
    /// <param name="cancellationToken">token</param>
    /// <returns>A task that represents the asynchronous save operation.
    /// The task result contains the number of state entries written to the database</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        DateTracking();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    /// Method which called when entity saved
    /// </summary>
    /// <returns>The number of state entries written to the database</returns>
    public override int SaveChanges()
    {
        DateTracking();
        return base.SaveChanges();
    }
    
    /// <summary>
    /// Method for setting the desired date format
    /// </summary>
    private void DateTracking()
    {
        foreach (var entry in ChangeTracker.Entries<ICreatedAtTrackedEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTime.UtcNow;
                    break;
            }
        }
    }
}

/// <summary>
/// Context for mysql database
/// </summary>
public class KirelLogMysqlMessageContext : KirelLogMessageContext
{
    /// <summary>
    /// Returns an instance of the mysql database context
    /// </summary>
    /// <param name="dbContextOptions">The options for this context</param>
    public KirelLogMysqlMessageContext(DbContextOptions<KirelLogMysqlMessageContext> dbContextOptions) : base(dbContextOptions)
    {
    }
}
/// <summary>
/// Context for mssql database
/// </summary>
public class KirelLogMssqlMessageContext : KirelLogMessageContext
{
    /// <summary>
    /// Returns an instance of the mssql database context
    /// </summary>
    /// <param name="dbContextOptions">The options for this context</param>
    public KirelLogMssqlMessageContext(DbContextOptions<KirelLogMssqlMessageContext> dbContextOptions) : base(dbContextOptions)
    {
    }
}
/// <summary>
/// Context for postgresql database
/// </summary>
public class KirelLogPostgresqlMessageContext : KirelLogMessageContext
{
    /// <summary>
    /// Returns an instance of the postgresql database context
    /// </summary>
    /// <param name="dbContextOptions">The options for this context</param>
    public KirelLogPostgresqlMessageContext(DbContextOptions<KirelLogPostgresqlMessageContext> dbContextOptions) : base(dbContextOptions)
    {
    }
}