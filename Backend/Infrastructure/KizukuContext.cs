using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;

public class KizukuContext : DbContext
{
    public DbSet<User> Users { get; set; }
}