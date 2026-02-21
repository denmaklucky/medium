namespace Shared;

public sealed class ToDo
{
    public long Id { get; set; }

    public string? Title { get; set; }

    public bool IsDone { get; set; } = false;
}