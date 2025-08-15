using Microsoft.EntityFrameworkCore;

namespace DiceRolls.Models;

public class DataContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DataContext(IConfiguration configuration, DbContextOptions<DataContext> options) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Throw> Throw { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = _configuration.GetValue<string>("Database:Path");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Throw>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(p => p.Login)
                .HasMaxLength(25)
                .HasDefaultValue(null);
            e.Property(p => p.SessionId)
                .HasMaxLength(100);
            e.Property(p => p.DiceValues)
                .HasMaxLength(250);
        });
    }
}