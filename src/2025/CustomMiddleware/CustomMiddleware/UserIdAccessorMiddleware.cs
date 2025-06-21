using System.Security.Claims;

namespace CustomMiddleware;

public sealed class UserIdAccessorMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, IUserIdAccessor userIdAccessor)
    {
        if (context.User.Identity is ClaimsIdentity { IsAuthenticated: true } identity)
        {
            var idClaim = identity.FindFirst("id");
            userIdAccessor.UserId = Guid.TryParse(idClaim?.Value, out var userId) ? userId : null;
        }

        await next(context);
    }
}
