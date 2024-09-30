using FluentValidation;

namespace RestApi.Domain.Validators;

public class UserProfileValidator: AbstractValidator<User>
{
    public UserProfileValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("A Name is required");
        RuleFor(user => user.Email).NotEmpty().WithMessage("A Email is required")
            .EmailAddress().WithMessage("Enter an email with the correct format");
    }
}