using FluentValidation;
using LeafBidAPI.App.Domain.Buyer.Data;

namespace LeafBidAPI.App.Domain.Buyer.Validators;

public class GetBuyerValidator : AbstractValidator<GetBuyerData>
{
    public GetBuyerValidator() => RuleFor(x => x.Id).GreaterThan(0);
}
