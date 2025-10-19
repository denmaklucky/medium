namespace CSharpEfAndAspNet;

public static class ResultExtensions
{
    extension<TValue, TError>(Result<TValue, TError> result)
    {
        public bool IsError => !result.IsSuccess;

        // public TValue? GetValueOrDefault() => result.IsSuccess 
        //     ? result.Value 
        //     : default;

        public static Result<TValue, NotFound> NotFound() => Result<TValue, NotFound>.Error(new NotFound());
    }
}