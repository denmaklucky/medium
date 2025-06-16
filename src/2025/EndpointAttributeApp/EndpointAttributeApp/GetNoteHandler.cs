using EndpointGenerator;

namespace EndpointAttributeApp;

[Endpoint("Get", "api/v1/notes")]
public sealed class GetNoteHandler : IHandler<object, object>
{
    public Task<object> InvokeAsync(object command)
    {
        throw new NotImplementedException();
    }
}