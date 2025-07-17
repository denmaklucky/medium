using Bogus;
using Microsoft.EntityFrameworkCore;

namespace GraphQlApp;

internal static class DbSeeder
{
    public static async Task SeedAsync(WeatherDbContext dbContext)
    {
        if (await dbContext.Countries.AnyAsync())
        {
            return;
        }
        
        Randomizer.Seed = new Random(12345);
        
        var countries = GenerateCountries();
        await dbContext.Countries.AddRangeAsync(countries);
        await dbContext.SaveChangesAsync();
        
        var cities = GenerateCities(countries);
        await dbContext.Cities.AddRangeAsync(cities);
        await dbContext.SaveChangesAsync();

        var weatherForecasts = GenerateWeatherForecasts(cities);
        await dbContext.WeatherForecasts.AddRangeAsync(weatherForecasts);
        await dbContext.SaveChangesAsync();
    }
    
    private static Country[] GenerateCountries()
    {
        return
        [
            new()  { CountryCode = "US" },
            new()  { CountryCode = "CA" },
            new()  { CountryCode = "GB" },
            new()  { CountryCode = "DE" },
            new()  { CountryCode = "FR" },
            new()  { CountryCode = "JP" },
            new()  { CountryCode = "AU" },
            new()  { CountryCode = "BR" },
            new()  { CountryCode = "IN" },
            new()  { CountryCode = "CN" }
        ];
    }
    
    
    private static List<City> GenerateCities(Country[] countries)
    {
        var cityData = new Dictionary<string, string[]>
        {
            ["US"] =
            [
                "New York",
                "Los Angeles",
                "Chicago",
                "Houston",
                "Phoenix"
            ],
            ["CA"] =
            [
                "Toronto",
                "Vancouver",
                "Montreal",
                "Calgary",
                "Ottawa"
            ],
            ["GB"] =
            [
                "London",
                "Manchester",
                "Birmingham",
                "Edinburgh",
                "Liverpool"
            ],
            ["DE"] =
            [
                "Berlin",
                "Munich",
                "Hamburg",
                "Frankfurt",
                "Cologne"
            ],
            ["FR"] =
            [
                "Paris",
                "Lyon",
                "Marseille",
                "Toulouse",
                "Nice"
            ]
        };
        
        var cities = new List<City>();
        
        foreach (var country in countries)
        {
            if (cityData.TryGetValue(country.CountryCode, out var countryCities))
            {
                cities.AddRange(countryCities.Select(cityInfo => new City { Name = cityInfo, CountryId = country.Id }));
            }
            else
            {
                var cityFaker = new Faker<City>()
                    .RuleFor(c => c.Name, f => f.Address.City())
                    .RuleFor(c => c.CountryId, country.Id);
                
                cities.AddRange(cityFaker.Generate(3));
            }
        }
        
        return cities;
    }
    
    private static List<WeatherForecast> GenerateWeatherForecasts(List<City> cities)
    {
        var weatherDescriptions = new[]
        {
            "Clear sky",
            "Partly cloudy",
            "Cloudy",
            "Overcast",
            "Light rain",
            "Heavy rain",
            "Thunderstorm",
            "Snow",
            "Fog",
            "Mist",
            "Sunny",
            "Windy"
        };
        
        var forecastFaker = new Faker<WeatherForecast>()
            .RuleFor(wf => wf.TemperatureHigh, f => f.Random.Double(-15, 50))
            .RuleFor(wf => wf.TemperatureLow, (f, wf) => f.Random.Double(-25, wf.TemperatureHigh - 5))
            .RuleFor(wf => wf.Humidity, f => f.Random.Double(20, 100))
            .RuleFor(wf => wf.Pressure, f => f.Random.Double(980, 1040))
            .RuleFor(wf => wf.WindSpeed, f => f.Random.Double(0, 50))
            .RuleFor(wf => wf.WindDirection, f => f.Random.Int(0, 360))
            .RuleFor(wf => wf.Description, f => f.PickRandom(weatherDescriptions));
        
        var weatherForecasts = new List<WeatherForecast>();
        
        foreach (var city in cities)
        {
            // Generate 7-14 day forecasts for each city
            var forecastDays = new Random().Next(7, 15);
            
            for (int i = 1; i <= forecastDays; i++)
            {
                var forecast = forecastFaker
                    .RuleFor(wf => wf.CityId, city.Id)
                    .RuleFor(wf => wf.ForecastDate, DateTime.Today.AddDays(i))
                    .Generate();
                
                weatherForecasts.Add(forecast);
            }
        }
        
        return weatherForecasts;
    }
}