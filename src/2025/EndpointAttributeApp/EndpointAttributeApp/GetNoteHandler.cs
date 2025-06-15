namespace EndpointAttributeApp;

[Endpoint(HttpVerb.GET, "api/v1/notes/")]
public sealed class GetNoteHandler : IHandler<object, object>
{
    public Task<object> InvokeAsync(object command)
    {
        throw new NotImplementedException();
    }
}