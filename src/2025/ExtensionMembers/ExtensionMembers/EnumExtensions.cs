namespace ExtensionMembers;

public static class EnumExtensions
{
    extension<T>(T) where T : struct, Enum 
    {
        public static T Parse(string value) =>
            Enum.Parse<T>(value);
    }
}