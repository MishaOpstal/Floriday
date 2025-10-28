using FluentValidation;
using LeafBidAPI.App.Domain.AuctionSale.Data;

namespace LeafBidAPI.App.Domain.AuctionSale.Validators;

public class GetAuctionSaleValidator : AbstractValidator<GetAuctionSaleData>
{
    public GetAuctionSaleValidator() => RuleFor(x => x.Id).GreaterThan(0);
}