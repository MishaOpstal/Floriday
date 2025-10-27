using FluentValidation;
using LeafBidAPI.Domain.Auction.Data;

namespace LeafBidAPI.Application.Domain.Auction.Validators;

public class GetAuctionValidator : AbstractValidator<GetAuctionData>
{
    public GetAuctionValidator() => RuleFor(x => x.Id).GreaterThan(0);
}
