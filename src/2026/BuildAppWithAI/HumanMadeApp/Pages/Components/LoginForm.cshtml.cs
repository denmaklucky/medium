using System.ComponentModel.DataAnnotations;
using Hydro;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HumanMadeApp.Pages.Components;

public class LoginForm(UserRepository repository, PasswordHasher<string> passwordHasher) : HydroComponent
{
    public string? ErrorMessage { get; set; }

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    public bool ShowPassword { get; set; }

    public async Task LoginAsync()
    {
        if (!Validate())
        {
            ErrorMessage = "Invalid username or password";

            return;
        }

        var getUerResult = await repository.GetAsync(Username);

        if (getUerResult is GetUserResult.NotFound)
        {
            ErrorMessage = "User not found";

            return;
        }

        if (getUerResult is not GetUserResult.Success success)
        {
            ErrorMessage = "Unknown error";

            return;
        }

        var verifyResult = passwordHasher.VerifyHashedPassword(Username, success.Hash, Password);

        if (verifyResult == PasswordVerificationResult.Failed)
        {
            ErrorMessage = "Incorrect password";

            return;
        }

        await AuthHelper.SignInAsync(HttpContext, success.UserId, Username);

        Location(Url.Page("/Index"));
    }

    public void NavigateToSignUp()
    {
        Location(Url.Page("/SignUp"));
    }
}
