using DataAccess;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FEndpoints;

public sealed class GetUsersEndpoint(IUsersService service) : EndpointWithoutRequest<Ok<IReadOnlyCollection<User>>>
{
    public override void Configure()
    {
        Get("api/v1/home");
        AllowAnonymous();
    }

    public override async Task<Ok<IReadOnlyCollection<User>>> ExecuteAsync(
        CancellationToken ct)
    {
        var users = await service.GetAsync();

        return TypedResults.Ok(users);
    }
}