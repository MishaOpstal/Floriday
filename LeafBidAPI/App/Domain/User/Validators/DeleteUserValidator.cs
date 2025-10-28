using FluentValidation;
using LeafBidAPI.App.Domain.User.Data;

namespace LeafBidAPI.App.Domain.User.Validators;

public class DeleteUserValidator : AbstractValidator<DeleteUserData>
{
    public DeleteUserValidator() =>
        RuleFor(x => x.Id).GreaterThan(0);
}