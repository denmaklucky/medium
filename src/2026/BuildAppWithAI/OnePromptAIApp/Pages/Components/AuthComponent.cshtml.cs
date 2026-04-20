using Hydro;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnePromptAIApp.Data;
using System.Security.Claims;

namespace OnePromptAIApp.Pages.Components;

public class AuthComponent(UserRepository users, IHttpContextAccessor httpContextAccessor) : HydroComponent
{
    public string ActiveTab { get; set; } = "login";

    // Login
    public string LoginUsername { get; set; } = "";
    public string LoginPassword { get; set; } = "";
    public string LoginError { get; set; } = "";

    // Sign-up
    public string SignupUsername { get; set; } = "";
    public string SignupPassword { get; set; } = "";
    public string SignupConfirmPassword { get; set; } = "";
    public string SignupError { get; set; } = "";

    public void SwitchToLogin()
    {
        ActiveTab = "login";
        LoginError = "";
        SignupError = "";
    }

    public void SwitchToSignup()
    {
        ActiveTab = "signup";
        LoginError = "";
        SignupError = "";
    }

    public async Task Login()
    {
        LoginError = "";
        if (string.IsNullOrWhiteSpace(LoginUsername) || string.IsNullOrWhiteSpace(LoginPassword))
        {
            LoginError = "Please fill in all fields.";
            return;
        }

        var user = await users.FindByUsernameAsync(LoginUsername.Trim());
        if (user == null || !BCrypt.Net.BCrypt.Verify(LoginPassword, user.Hash))
        {
            LoginError = "Invalid username or password.";
            return;
        }

        await SignInUser(user);
        httpContextAccessor.HttpContext!.Response.Redirect("/Dashboard");
    }

    public async Task SignUp()
    {
        SignupError = "";
        if (string.IsNullOrWhiteSpace(SignupUsername) || string.IsNullOrWhiteSpace(SignupPassword))
        {
            SignupError = "Please fill in all fields.";
            return;
        }
        if (SignupPassword != SignupConfirmPassword)
        {
            SignupError = "Passwords do not match.";
            return;
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(SignupPassword);
        var created = await users.CreateAsync(SignupUsername.Trim(), hash);
        if (!created)
        {
            SignupError = "Username already taken.";
            return;
        }

        var user = await users.FindByUsernameAsync(SignupUsername.Trim());
        await SignInUser(user!);
        httpContextAccessor.HttpContext!.Response.Redirect("/Dashboard");
    }

    private async Task SignInUser(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Username)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContextAccessor.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));
    }
}
