using System.ComponentModel.DataAnnotations;
using Hydro;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HumanMadeApp.Pages.Components;

public class RegisterForm(UserRepository repository, PasswordHasher<string> passwordHasher) : HydroComponent
{
    public string? ErrorMessage { get; set; }

    public bool ShowPassword { get; set; }

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string ConfirmPassword { get; set; } = null!;

    public async Task RegisterUserAsync()
    {
        if (!Validate())
        {
            ErrorMessage = "Please input a valid username and password";

            return;
        }

        if (!string.Equals(ConfirmPassword, Password))
        {
            ErrorMessage = "Passwords do not match";

            return;
        }

        var hash = passwordHasher.HashPassword(Username, Password);

        var result = await repository.RegisterAsync(Username, hash);

        if (result is RegisterUserResult.UserAlreadyRegistered)
        {
            ErrorMessage = "User is already registered. Please use a different name";

            return;
        }

        if (result is not RegisterUserResult.Success success)
        {
            ErrorMessage = "Unknown error. Please try again";

            return;
        }

        await AuthHelper.SignInAsync(HttpContext, Username, success.Id.ToString());
        Location(Url.Page("/Index"));
    }

    public void NavigateToSignIn()
    {
        Location(Url.Page("/SignIn"));
    }
}
