using DataAccess;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FEndpoints;

public sealed class DeleteUserEndpoint(IUsersService service) : EndpointWithoutRequest<NoContent>
{
    public override void Configure()
    {
        Delete("api/v1/home");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await service.DeleteFirstAsync();

        await SendAsync(TypedResults.NoContent(), cancellation: ct);
    }
}