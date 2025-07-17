namespace GraphQlApp;

public sealed class Country
{
    public int Id { get; set; }
    
    public string CountryCode { get; set; } = null!;
    
    public List<City> Cities { get; set; } = [];
}