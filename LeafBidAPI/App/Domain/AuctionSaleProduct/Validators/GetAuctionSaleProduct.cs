using FluentValidation;
using LeafBidAPI.App.Domain.AuctionSaleProduct.Data;

namespace LeafBidAPI.App.Domain.AuctionSaleProduct.Validators;

public class GetAuctionSalesProductValidator : AbstractValidator<GetAuctionSaleProductData>
{
    public GetAuctionSalesProductValidator() => RuleFor(x => x.Id).GreaterThan(0);
}