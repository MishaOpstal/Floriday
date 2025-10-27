using FluentValidation;
using LeafBidAPI.Domain.AuctionSale.Data;

namespace LeafBidAPI.Application.Domain.AuctionSale.Validators;

public class GetAuctionSaleValidator : AbstractValidator<GetAuctionSaleData>
{
    public GetAuctionSaleValidator() => RuleFor(x => x.Id).GreaterThan(0);
}