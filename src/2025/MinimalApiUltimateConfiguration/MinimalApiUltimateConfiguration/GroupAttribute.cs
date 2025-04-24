namespace MinimalApiUltimateConfiguration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GroupAttribute : Attribute
{
    public GroupAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "Group name cannot be null or empty.");
        }

        Name = $"/{name.TrimStart('/')}";
    }
    
    public string Name { get; }
}