using FluentValidation;
using LeafBidAPI.Domain.Auctioneer.Data;

namespace LeafBidAPI.Application.Domain.Auctioneer.Validators;

public class DeleteAuctioneerValidator : AbstractValidator<DeleteAuctioneerData>
{
    public DeleteAuctioneerValidator() => RuleFor(x => x.Id).GreaterThan(0);
}