namespace BestIdentifier;

public struct Suid : IComparable<Suid>
{
    private const string Symbols = "0123456789ABCDEFGHIJKLMNPQRSTUVWXYabcdefghjklmnopqrstuvwxyz";
    private const int SymbolsCount = 59;

    private static long _lastTicks;
    private static int _counter;

    public static readonly Suid Empty = new(0);

    private readonly long _value;

    private Suid(long value) => _value = value;

    public static Suid New()
    {
        var currentTicks = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        Interlocked.CompareExchange(ref _lastTicks, currentTicks, _lastTicks);

        if (currentTicks == _lastTicks)
        {
            Interlocked.Increment(ref _counter);
        }
        else
        {
            Interlocked.Exchange(ref _counter, 0);
            _lastTicks = currentTicks;
        }

        var random = Random.Shared.NextInt64(maxValue: currentTicks);

        var value = currentTicks + _counter + random + Environment.CurrentManagedThreadId;

        return new(value);
    }

    public static Suid Parse(long value) => new(value);

    public static Suid Parse(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        long result = 0;

        foreach (var symbol in value)
        {
            result *= SymbolsCount;
            result += Symbols.IndexOf(symbol);
        }

        return new(result);
    }

    public override string ToString()
    {
        if (_value == 0)
            return "0";

        var value = _value;
        var result = string.Empty;

        var array = BitConverter.GetBytes(value);

        while (value > 0)
        {
            result = Symbols[(int)(value % SymbolsCount)] + result;
            value /= SymbolsCount;
        }

        return result;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Suid suid)
        {
            return false;
        }

        return _value == suid._value;
    }

    public override int GetHashCode() => _value.GetHashCode();

    public int CompareTo(Suid other) => _value.CompareTo(other._value);

    public static bool operator ==(Suid left, Suid right) => left._value.Equals(right._value);

    public static bool operator !=(Suid left, Suid right) => !left._value.Equals(right._value);
}