using DataAccess;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FEndpoints;

public sealed class CreateUserEndpoint(IUsersService service) : Endpoint<CreateUserRequest, NoContent>
{
    public override void Configure()
    {
        Post("api/v1/home");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        await service.CreateAsync(req.Email);

        await SendAsync(TypedResults.NoContent(), cancellation: ct);
    }
}