using FluentValidation;
using LeafBidAPI.Domain.User.Data;

namespace LeafBidAPI.Application.Domain.User.Validators;

public class GetUserValidator : AbstractValidator<GetUserData>
{
    public GetUserValidator() =>
        RuleFor(x => x.Id).GreaterThan(0);
}