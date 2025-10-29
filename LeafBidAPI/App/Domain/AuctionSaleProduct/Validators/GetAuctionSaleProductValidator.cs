using FluentValidation;
using LeafBidAPI.App.Domain.AuctionSaleProduct.Data;

namespace LeafBidAPI.App.Domain.AuctionSaleProduct.Validators;

public class GetAuctionSaleProductValidator : AbstractValidator<GetAuctionSaleProductData>
{
    public GetAuctionSaleProductValidator() => RuleFor(x => x.Id).GreaterThan(0);
}