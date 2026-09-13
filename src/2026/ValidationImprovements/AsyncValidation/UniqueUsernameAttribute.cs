using System.ComponentModel.DataAnnotations;

namespace AsyncValidation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class UniqueUsernameAttribute : AsyncValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
        throw new InvalidOperationException($"Validate the attribute with {nameof(IsValidAsync)}.");

    protected override async Task<ValidationResult?> IsValidAsync(object? value, ValidationContext validationContext, CancellationToken cancellationToken)
    {
        var userService = validationContext.GetRequiredService<IUserService>();

        if (value is string username &&
            await userService.ExistAsync(username))
        {
            return new ValidationResult($"User with `{username}` already exists.");
        }

        return ValidationResult.Success;
    }
}
