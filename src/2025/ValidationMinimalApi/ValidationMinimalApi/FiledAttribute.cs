using System.ComponentModel.DataAnnotations;

namespace ValidationMinimalApi;

[AttributeUsage(AttributeTargets.Property)]
public sealed class FilledAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not Guid guid)
        {
            return false;
        }

        return guid != Guid.Empty;
    }
}