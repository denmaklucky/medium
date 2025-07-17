using Microsoft.EntityFrameworkCore;

namespace GraphQlApp;

public sealed class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries { get; set; }
    
    public DbSet<City> Cities { get; set; }
    
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(country => country.Id);
            entity.HasIndex(country => country.CountryCode).IsUnique();
            entity.Property(country => country.CountryCode).IsRequired().HasMaxLength(3);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(city => city.Id);
            entity.Property(city => city.Name).IsRequired().HasMaxLength(100);
            entity.HasOne(city => city.Country)
                .WithMany(country => country.Cities)
                .HasForeignKey(city => city.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(weatherForecast => weatherForecast.Id);
            entity.Property(weatherForecast => weatherForecast.Description).HasMaxLength(100);
            entity.HasOne(weatherForecast => weatherForecast.City)
                .WithMany(city => city.WeatherForecasts)
                .HasForeignKey(weatherForecast => weatherForecast.CityId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        base.OnModelCreating(modelBuilder);
    }
}