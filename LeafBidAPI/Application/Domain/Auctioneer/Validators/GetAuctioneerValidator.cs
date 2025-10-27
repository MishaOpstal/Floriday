using FluentValidation;
using LeafBidAPI.Domain.Auctioneer.Data;

namespace LeafBidAPI.Application.Domain.Auctioneer.Validators;

public class GetAuctioneerValidator : AbstractValidator<GetAuctioneerData>
{
    public GetAuctioneerValidator() => RuleFor(x => x.Id).GreaterThan(0);
}