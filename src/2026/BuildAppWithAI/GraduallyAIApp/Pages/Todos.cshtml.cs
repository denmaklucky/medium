using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GraduallyAIApp.Pages;

[Authorize]
public class TodosModel : PageModel
{
    public void OnGet()
    {
    }
}
