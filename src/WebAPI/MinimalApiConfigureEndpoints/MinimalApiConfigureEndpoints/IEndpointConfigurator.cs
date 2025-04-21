namespace MinimalApiConfigureEndpoints;

public interface IEndpointConfigurator
{
    void Configure(IEndpointRouteBuilder routeBuilder);
}