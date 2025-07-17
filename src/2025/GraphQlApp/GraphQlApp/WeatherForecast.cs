namespace GraphQlApp;

public sealed class WeatherForecast
{
    public int Id { get; set; }
    
    public double TemperatureHigh { get; set; }
    
    public double TemperatureLow { get; set; }
    
    public double Humidity { get; set; }
    
    public double Pressure { get; set; }
    
    public double WindSpeed { get; set; }
    
    public int WindDirection { get; set; }
    
    public string Description { get; set; } = null!;
    
    public DateTime ForecastDate { get; set; }
    
    public int CityId { get; set; }
    
    public City? City { get; set; }
}