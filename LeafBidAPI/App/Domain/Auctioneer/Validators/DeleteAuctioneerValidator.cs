using FluentValidation;
using LeafBidAPI.App.Domain.Auctioneer.Data;

namespace LeafBidAPI.App.Domain.Auctioneer.Validators;

public class DeleteAuctioneerValidator : AbstractValidator<DeleteAuctioneerData>
{
    public DeleteAuctioneerValidator() => RuleFor(x => x.Id).GreaterThan(0);
}