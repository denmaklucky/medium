using Microsoft.EntityFrameworkCore;

namespace GraphQlApp;

public class WeatherQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Country> GetCountries([Service] WeatherDbContext context)
        => context.Countries;
    
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<City> GetCities([Service] WeatherDbContext context)
        => context.Cities;
    
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<WeatherForecast> GetWeatherForecasts([Service] WeatherDbContext context)
        => context.WeatherForecasts;
    
    public Task<Country?> GetCountryByCode(string countryCode, [Service] WeatherDbContext context)
        => context.Countries.FirstOrDefaultAsync(country => country.CountryCode == countryCode);
    
    public Task<List<City>> GetCitiesByCountry(int countryId, [Service] WeatherDbContext context)
        => context.Cities.Where(city => city.CountryId == countryId).ToListAsync();
    
    public Task<List<WeatherForecast>> GetForecastsForCity(int cityId, int days, [Service] WeatherDbContext context)
        => context.WeatherForecasts
            .Where(weatherForecast => weatherForecast.CityId == cityId &&
                                      weatherForecast.ForecastDate >= DateTime.Today &&
                                      weatherForecast.ForecastDate <= DateTime.Today.AddDays(days))
            .OrderBy(wf => wf.ForecastDate)
            .ToListAsync();
}