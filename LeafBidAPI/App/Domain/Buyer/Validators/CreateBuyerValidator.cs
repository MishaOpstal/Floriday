using FluentValidation;
using LeafBidAPI.App.Domain.Buyer.Data;

namespace LeafBidAPI.App.Domain.Buyer.Validators;

public class CreateBuyerValidator : AbstractValidator<CreateBuyerData>
{
    public CreateBuyerValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(255);
    }
}