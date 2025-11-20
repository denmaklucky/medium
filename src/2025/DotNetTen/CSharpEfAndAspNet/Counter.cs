namespace CSharpEfAndAspNet;

public sealed class Counter(int value)
{
    public int Value { get; } = value;
    
    public static Counter operator ++(Counter counter)
    {
        var currentValue = counter.Value;

        currentValue += 1;

        return new (currentValue);
    }

    public static Counter operator --(Counter counter)
    {
        var currentValue = counter.Value;

        currentValue -= 1;

        return new (currentValue);
    }
}