using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;

public interface IKizukuContext
{
    DbSet<User> Users { get; set; }
    /// <summary>
    /// Asynchronously saves all changes made in the context to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}


public class KizukuContext(DbContextOptions<KizukuContext> options)
    : DbContext(options), IKizukuContext
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
