using FluentValidation;
using LeafBidAPI.App.Domain.Buyer.Data;

namespace LeafBidAPI.App.Domain.Buyer.Validators;

public class UpdateBuyerValidator : AbstractValidator<UpdateBuyerData>
{
    public UpdateBuyerValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        When(x => x.UserId.HasValue, () => RuleFor(x => x.UserId!.Value).GreaterThan(0));
        When(x => x.CompanyName != null, () => RuleFor(x => x.CompanyName!).MaximumLength(255));
    }
}