using Microsoft.EntityFrameworkCore;
using FarmAdvisor.Models;
using FarmAdvisor.Commons;

namespace FarmAdvisor.DataAccess.MSSQL;

public class FarmAdvisorDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Farm> Farms { get; set; } = null!;
    public DbSet<Field> Fields { get; set; } = null!;
    public DbSet<Sensor> Sensors { get; set; } = null!;
    public DbSet<SensorGddReset> SensorGddResets { get; set; } = null!;
    public DbSet<SensorStatistic> SensorStatistics { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,5500;Database=FarmAdvisor;User Id=SA;Password="+DbCommons.getPassWord()+";MultipleActiveResultSets=true;TrustServerCertificate=true");
    }
}