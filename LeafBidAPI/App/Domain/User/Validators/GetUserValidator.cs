using FluentValidation;
using LeafBidAPI.App.Domain.User.Data;

namespace LeafBidAPI.App.Domain.User.Validators;

public class GetUserValidator : AbstractValidator<GetUserData>
{
    public GetUserValidator() =>
        RuleFor(x => x.Id).GreaterThan(0);
}