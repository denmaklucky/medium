using System.Security.Claims;
using Hydro;
using Microsoft.AspNetCore.Authentication;
using OneShotAIApp.Data;
using OneShotAIApp.Models;

namespace OneShotAIApp.Pages.Components;

public class SignUpForm : HydroComponent
{
    private readonly UserRepository _userRepo;

    public SignUpForm(UserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string? Error { get; set; }

    public async Task SignUp()
    {
        Error = null;

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Please fill in all fields.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            Error = "Passwords do not match.";
            return;
        }

        if (Password.Length < 6)
        {
            Error = "Password must be at least 6 characters.";
            return;
        }

        var existing = await _userRepo.GetByUsernameAsync(Username.Trim());
        if (existing != null)
        {
            Error = "Username is already taken.";
            return;
        }

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Username = Username.Trim(),
            Hash = BCrypt.Net.BCrypt.HashPassword(Password)
        };

        await _userRepo.CreateAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("Cookies", principal);
        Location("/Dashboard");
    }
}
