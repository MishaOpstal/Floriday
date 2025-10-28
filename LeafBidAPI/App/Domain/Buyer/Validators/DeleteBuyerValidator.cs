using FluentValidation;
using LeafBidAPI.App.Domain.Buyer.Data;

namespace LeafBidAPI.App.Domain.Buyer.Validators;

public class DeleteBuyerValidator : AbstractValidator<DeleteBuyerData>
{
    public DeleteBuyerValidator() => RuleFor(x => x.Id).GreaterThan(0);
}