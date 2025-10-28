using FluentValidation;
using LeafBidAPI.App.Domain.Auctioneer.Data;

namespace LeafBidAPI.App.Domain.Auctioneer.Validators;

public class GetAuctioneerValidator : AbstractValidator<GetAuctioneerData>
{
    public GetAuctioneerValidator() => RuleFor(x => x.Id).GreaterThan(0);
}