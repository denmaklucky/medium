using System.Linq.Expressions;

namespace CSharpEfAndAspNet;

public static class ResultMethodsExtensions
{
    public static TValue? GetValueOrDefault<TValue, TError>(this Result<TValue, TError> result, TValue? defaultValue = default)
    {
        if (result.IsSuccess)
        {
            return result.Value;
        }
        
        return defaultValue is not null
            ? defaultValue
            : default;
    }
}

public class Test
{
    public void Invoke<TValue, TError>()
    {
        Expression<Func<Result<TValue, TError>, TValue, TValue>> setDefault;
        
        // setDefault = (result, defaultValue) => result.GetValueOrDefault(defaultValue);
        // setDefault = (result, defaultValue) => result.GetValueOrDefault(defaultValue: defaultValue);
    }
}