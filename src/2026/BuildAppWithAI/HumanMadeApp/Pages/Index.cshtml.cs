using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanMadeApp.Pages;

[Authorize]
public class IndexModel : PageModel
{
    public void OnGet()
    {

    }
}
