namespace HumanMadeApp;

public static class CookieHelper
{
    public static void Set(HttpContext context, string key, string value, int expiryDays = 7)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(expiryDays)
        };

        context.Response.Cookies.Append(key, value, options);
    }

    public static bool TryGet(HttpContext context, string key, out string? value)
    {
        value = context.Request.Cookies[key];

        return !string.IsNullOrEmpty(value);
    }

    public static void Remove(HttpContext context, string key)
    {
        context.Response.Cookies.Delete(key);
    }
}
