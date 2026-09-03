using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FeelSpaApp.Pages;

public class Notification : PageModel
{
    public void OnGet()
    {
    }

    public PartialViewResult OnGetNotification()
    {
        return Partial("_Notification", new Message
        {
            Text = "Your changes have been saved.",
            CreatedAt = DateTimeOffset.Now
        });
    }

    public sealed class Message
    {
        public string Text { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }
    }
}
