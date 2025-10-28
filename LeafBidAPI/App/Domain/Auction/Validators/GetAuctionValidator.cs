using FluentValidation;
using LeafBidAPI.App.Domain.Auction.Data;

namespace LeafBidAPI.App.Domain.Auction.Validators;

public class GetAuctionValidator : AbstractValidator<GetAuctionData>
{
    public GetAuctionValidator() => RuleFor(x => x.Id).GreaterThan(0);
}
