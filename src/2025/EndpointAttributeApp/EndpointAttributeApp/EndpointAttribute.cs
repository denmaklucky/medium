namespace EndpointAttributeApp;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EndpointAttribute(HttpVerb verb, string route) : Attribute
{
    public HttpVerb Verb { get; } = verb;

    public string Route { get; } = route;
}