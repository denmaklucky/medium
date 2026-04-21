using System.Security.Claims;
using Hydro;
using Microsoft.AspNetCore.Authentication;
using OneShotAIApp.Data;

namespace OneShotAIApp.Pages.Components;

public class LoginForm : HydroComponent
{
    private readonly UserRepository _userRepo;

    public LoginForm(UserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Error { get; set; }

    public async Task Login()
    {
        Error = null;

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Please fill in all fields.";
            return;
        }

        var user = await _userRepo.GetByUsernameAsync(Username.Trim());
        if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.Hash))
        {
            Error = "Invalid username or password.";
            return;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("Cookies", principal);
        Location("/Dashboard");
    }
}
