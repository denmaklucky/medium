using System.Text.Json;

var json = args[0];

try
{
    var jsonDocument = JsonDocument.Parse(json);
    var options = new JsonSerializerOptions { WriteIndented = true };
    var pretty = JsonSerializer.Serialize(jsonDocument.RootElement, options);
    Console.WriteLine(pretty);
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Invalid JSON: {exception.Message}");
}