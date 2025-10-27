using FluentValidation;
using LeafBidAPI.Domain.AuctionSale.Data;

namespace LeafBidAPI.Application.Domain.AuctionSale.Validators;

public class CreateAuctionSaleValidator : AbstractValidator<CreateAuctionSaleData>
{
    public CreateAuctionSaleValidator()
    {
        RuleFor(x => x.AuctionId).GreaterThan(0);
        RuleFor(x => x.BuyerId).GreaterThan(0);
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.PaymentReference).NotEmpty().MaximumLength(255);
    }
}