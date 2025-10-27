using FluentValidation;
using LeafBidAPI.Domain.Buyer.Data;

namespace LeafBidAPI.Application.Domain.Buyer.Validators;

public class GetBuyerValidator : AbstractValidator<GetBuyerData>
{
    public GetBuyerValidator() => RuleFor(x => x.Id).GreaterThan(0);
}
