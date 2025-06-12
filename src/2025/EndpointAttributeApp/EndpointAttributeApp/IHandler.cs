namespace EndpointAttributeApp;

public interface IHandler<in TCommand, TResult>
{
    Task<TResult> InvokeAsync(TCommand command);
}