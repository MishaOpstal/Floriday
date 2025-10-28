using FluentValidation;
using LeafBidAPI.App.Domain.Auctioneer.Data;

namespace LeafBidAPI.App.Domain.Auctioneer.Validators;

public class CreateAuctioneerValidator : AbstractValidator<CreateAuctioneerData>
{
    public CreateAuctioneerValidator() => RuleFor(x => x.UserId).GreaterThan(0);
}