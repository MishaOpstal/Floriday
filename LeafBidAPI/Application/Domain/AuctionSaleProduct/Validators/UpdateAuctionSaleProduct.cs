using FluentValidation;
using LeafBidAPI.Domain.AuctionSaleProduct.Data;

namespace LeafBidAPI.Application.Domain.AuctionSaleProduct.Validators;

public class UpdateAuctionSalesProductValidator : AbstractValidator<UpdateAuctionSaleProductData>
{
    public UpdateAuctionSalesProductValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        When(x => x.AuctionSaleId.HasValue, () => RuleFor(x => x.AuctionSaleId!.Value).GreaterThan(0));
        When(x => x.ProductId.HasValue, () => RuleFor(x => x.ProductId!.Value).GreaterThan(0));
        When(x => x.Quantity.HasValue, () => RuleFor(x => x.Quantity!.Value).GreaterThan(0));
        When(x => x.Price.HasValue, () => RuleFor(x => x.Price!.Value).GreaterThanOrEqualTo(0));
    }
}