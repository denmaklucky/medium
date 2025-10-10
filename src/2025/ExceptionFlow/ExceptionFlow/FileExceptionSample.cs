namespace ExceptionFlow;

internal class FileExceptionSample
{
    private const string _fileName = "test.txt";
    
    public async Task ReadFileWithoutCatch()
    {
        var text = await File.ReadAllTextAsync(_fileName);
    }
    
    public async Task ReadFileWithCatch()
    {
        try
        {
            var text = await File.ReadAllTextAsync(_fileName);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}