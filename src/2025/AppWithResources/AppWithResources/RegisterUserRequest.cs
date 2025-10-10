using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AppWithResources;

public sealed class RegisterUserRequest
{
    [MaxLength(50, ErrorMessageResourceType = typeof(ValidateError))]
    public string Username { get; set; } = null!;
    
    [Required(ErrorMessageResourceType = typeof(ValidateError))]
    public string Password { get; set; } = null!;
}

public sealed class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator(IStringLocalizer<ValidateError> localizer)
    {
        RuleFor(x => x.Username).MaximumLength(50).WithMessage(localizer["MaxLengthField"]);
        RuleFor(x => x.Password).NotEmpty().WithMessage(localizer["RequiredField"]);
    }
}