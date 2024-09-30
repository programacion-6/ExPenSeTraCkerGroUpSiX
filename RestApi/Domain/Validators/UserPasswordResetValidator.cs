using FluentValidation;

namespace RestApi.Domain.Validators;

public class UserPasswordResetValidator : AbstractValidator<User.PasswordUpdate>
{
    public UserPasswordResetValidator()
    {
        RuleFor(user => user.Password).NotEmpty().WithMessage("A Name is required");
        RuleFor(user => user.Code).NotEmpty().WithMessage("A Code is required");
    }
}