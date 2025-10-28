using FluentValidation;
using LeafBidAPI.App.Domain.Auctioneer.Data;

namespace LeafBidAPI.App.Domain.Auctioneer.Validators;

public class UpdateAuctioneerValidator : AbstractValidator<UpdateAuctioneerData>
{
    public UpdateAuctioneerValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        When(x => x.UserId.HasValue, () => RuleFor(x => x.UserId!.Value).GreaterThan(0));
    }
}