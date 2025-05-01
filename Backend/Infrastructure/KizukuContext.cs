using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;

public class KizukuContext(DbContextOptions<KizukuContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}