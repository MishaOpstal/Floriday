using FluentValidation;
using LeafBidAPI.App.Domain.AuctionSaleProduct.Data;

namespace LeafBidAPI.App.Domain.AuctionSaleProduct.Validators;

public class CreateAuctionSaleProductValidator : AbstractValidator<CreateAuctionSaleProductData>
{
    public CreateAuctionSaleProductValidator()
    {
        RuleFor(x => x.AuctionSaleId).GreaterThan(0);
        RuleFor(x => x.ProductId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}