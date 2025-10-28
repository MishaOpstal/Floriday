using FluentValidation;
using LeafBidAPI.App.Domain.User.Data;

namespace LeafBidAPI.App.Domain.User.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserData>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        When(x => x.Email is not null, () => RuleFor(x => x.Email!).EmailAddress());
        When(x => x.Password is not null, () => RuleFor(x => x.Password!).MinimumLength(8));
        RuleFor(x => x.Name).MaximumLength(255);
    }
}