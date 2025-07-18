﻿namespace GraphQlApp;

public sealed class City
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public int CountryId { get; set; }
    
    public Country? Country { get; set; }
    
    public List<WeatherForecast> WeatherForecasts { get; set; } = [];
}