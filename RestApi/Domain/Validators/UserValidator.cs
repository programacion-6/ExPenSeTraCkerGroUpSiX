namespace RestApi.Domain.Validators;
using FluentValidation;

public class UserValidator: AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(User => User.Name).NotEmpty().WithMessage("A Name is required");
        RuleFor(User => User.Email).NotEmpty().WithMessage("A Email is required")
            .EmailAddress().WithMessage("enter an email with the correct format");
        RuleFor(User => User.Password).NotEmpty().WithMessage("A Password is required");
    }
}
