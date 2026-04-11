using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanMadeApp.Pages;

public class SignIn : PageModel
{
    public ActionResult OnGet()
    {
        if (AuthHelper.IsAuthenticated(HttpContext))
        {
            return Redirect("/Index");
        }

        return Page();
    }
}
