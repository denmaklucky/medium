using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using WebApp.Options;

namespace WebApp.Pages;

public class Signup : PageModel
{
    private readonly AppOptions _options;

    public Signup(IOptions<AppOptions> options)
    {
        _options = options.Value;
    }
    
    public void OnGet()
    {
        
    }

    public string BaseUrl => _options.BaseUrl;
}