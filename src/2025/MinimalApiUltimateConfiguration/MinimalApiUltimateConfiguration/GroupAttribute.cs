namespace MinimalApiUltimateConfiguration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GroupAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}