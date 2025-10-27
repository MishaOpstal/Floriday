using FluentValidation;
using LeafBidAPI.Domain.AuctionSaleProduct.Data;

namespace LeafBidAPI.Application.Domain.AuctionSaleProduct.Validators;

public class GetAuctionSalesProductValidator : AbstractValidator<GetAuctionSaleProductData>
{
    public GetAuctionSalesProductValidator() => RuleFor(x => x.Id).GreaterThan(0);
}