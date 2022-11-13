using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class DataContext : DbContext
{
    public DbSet<CountedWall> CountedWalls { get; set; } = null!;

    public DataContext(DbContextOptions<DataContext> options)
       : base(options) => Database.Migrate();
}
