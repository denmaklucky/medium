namespace ExtensionMembers;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source) => source == null || !source.Any();

    // extension<T>(IEnumerable<T> source)
    // {
    //     public bool IsNullOrEmpty => source == null || !source.Any();
    // }
}