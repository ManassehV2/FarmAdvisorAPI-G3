using Microsoft.EntityFrameworkCore;
using FarmAdvisor.Models;

namespace FarmAdvisor.DataAccess.MSSQL;

public class FarmAdvisorDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=FarmAdvisor;User Id=SA;Password=Pass_Word;MultipleActiveResultSets=true;TrustServerCertificate=true");
    }
}