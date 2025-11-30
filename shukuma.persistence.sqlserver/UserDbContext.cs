using Microsoft.EntityFrameworkCore;

namespace shukuma.persistence.sqlserver;

public class UserDbContext : DbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DbSet<UserEntity> Users { get; set; }
}
