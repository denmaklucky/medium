using GraphQlApp;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WeatherDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OwlDb")));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<WeatherQuery>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
await DbSeeder.SeedAsync(context);

// app.MapGet("/countries", async ([FromServices] WeatherDbContext dbContext) =>
// {
//     var countries = await dbContext.Countries
//         .Select(country => new { country.CountryCode })
//         .ToListAsync();
//     
//     return TypedResults.Ok(countries);
// });
//
// app.MapGet("/countries/{countryCode}/cities", async (
//     [FromRoute] string countryCode,
//     [FromServices] WeatherDbContext dbContext) =>
// {
//     var cities = await dbContext.Cities
//         .Include(city => city.Country)
//         .Where(city => city.Country!.CountryCode == countryCode)
//         .Select(city => new { city.Id, city.Name })
//         .ToListAsync();
//     
//     return TypedResults.Ok(cities);
// });
//
// app.MapGet("/cities/{cityId:int}/forecast", async (
//     [FromRoute] int cityId,
//     [FromServices] WeatherDbContext dbContext) =>
// {
//     var forecast = await dbContext.WeatherForecasts
//         .Include(forecast => forecast.City)
//         .Where(forecast => forecast.City!.Id == cityId)
//         .Select(forecast => new
//         {
//             forecast.TemperatureHigh,
//             forecast.TemperatureLow,
//             forecast.Humidity,
//             forecast.Pressure,
//             forecast.WindSpeed,
//             forecast.WindDirection,
//             forecast.Description
//         })
//         .ToListAsync();
//     
//     return TypedResults.Ok(forecast);
// });

app.MapGraphQL();

app.Run();