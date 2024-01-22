using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Dal;

public class DataContext : DbContext
{
    #region Properties

    public DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;

    public DbSet<UserNote> UserNotes { get; set; } = null!;

    #endregion

    #region Constructors

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    #endregion

    #region Methods

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>().ToTable("Weather");

        modelBuilder.Entity<UserNote>().ToTable("UserNote");
    }

    #endregion
}