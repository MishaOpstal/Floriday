using FluentValidation;
using LeafBidAPI.App.Domain.AuctionSale.Data;

namespace LeafBidAPI.App.Domain.AuctionSale.Validators;

public class UpdateAuctionSaleValidator : AbstractValidator<UpdateAuctionSaleData>
{
    public UpdateAuctionSaleValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.PaymentReference).NotEmpty().MaximumLength(255);
    }
}