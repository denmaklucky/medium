using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HumanMadeApp;

public static class AuthHelper
{
    public static async Task SignInAsync(HttpContext context, string userId, string username)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, userId),
            new (ClaimTypes.Name, username)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

    public static async Task SignOutAsync(HttpContext context)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public static string? GetUserId(HttpContext context)
    {
        return context.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static string? GetUsername(HttpContext context)
    {
        return context.User.FindFirstValue(ClaimTypes.Name);
    }

    public static bool IsAuthenticated(HttpContext context)
    {
        return context.User.Identity?.IsAuthenticated ?? false;
    }
}
