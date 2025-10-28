using FluentValidation;
using LeafBidAPI.App.Domain.User.Data;

namespace LeafBidAPI.App.Domain.User.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserData>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.UserType).IsInEnum();
    }
}