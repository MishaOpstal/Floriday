using FluentValidation;
using LeafBidAPI.Domain.User.Data;

namespace LeafBidAPI.Application.Domain.User.Validators;

public class DeleteUserValidator : AbstractValidator<DeleteUserData>
{
    public DeleteUserValidator() =>
        RuleFor(x => x.Id).GreaterThan(0);
}