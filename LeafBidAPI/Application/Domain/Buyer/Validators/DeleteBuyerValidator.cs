using FluentValidation;
using LeafBidAPI.Domain.Buyer.Data;

namespace LeafBidAPI.Application.Domain.Buyer.Validators;

public class DeleteBuyerValidator : AbstractValidator<DeleteBuyerData>
{
    public DeleteBuyerValidator() => RuleFor(x => x.Id).GreaterThan(0);
}