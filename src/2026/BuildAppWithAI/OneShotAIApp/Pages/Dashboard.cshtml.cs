using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OneShotAIApp.Pages;

[Authorize]
public class DashboardModel : PageModel
{
    public void OnGet()
    {
    }
}
