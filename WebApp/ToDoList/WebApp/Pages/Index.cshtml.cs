using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Index : PageModel
{
    public IActionResult OnGet()
    {
        return RedirectToPage("Signin");
    }
}