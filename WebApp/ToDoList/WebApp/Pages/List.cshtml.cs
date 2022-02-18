using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using WebApp.Options;

namespace WebApp.Pages;

public class List : PageModel
{
    private readonly AppOptions _options;

    public List(IOptions<AppOptions> options)
    {
        _options = options.Value;
    }

    public void OnGet(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; set; }
    public string ApiKey => _options.ApiKey;
    public string BaseUrl => _options.BaseUrl;
}