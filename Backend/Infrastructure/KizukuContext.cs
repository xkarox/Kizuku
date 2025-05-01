using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;

public interface IKizukuContext
{
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}


public class KizukuContext(DbContextOptions<KizukuContext> options)
    : DbContext(options), IKizukuContext
{
    public DbSet<User> Users { get; set; }
}