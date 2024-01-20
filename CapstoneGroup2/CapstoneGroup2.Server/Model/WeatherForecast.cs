using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Model;

[EntityTypeConfiguration(typeof(WeatherForecastConfiguration))]
public class WeatherForecast
{
    #region Properties

    public DateOnly Date { get; set; }

    [Column("temperature")] public decimal TemperatureC { get; set; }

    public decimal TemperatureF => 32 + (int)(this.TemperatureC / (decimal)0.5556);

    public string? Summary { get; set; }

    #endregion
}

public class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
    #region Methods

    public void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        builder.ToTable("WeatherForecast");
        builder.HasKey(x => x.Date);
        builder.Property(x => x.TemperatureC).IsRequired();
        builder.Property(x => x.Summary).HasMaxLength(100);
    }

    #endregion
}