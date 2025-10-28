using FluentValidation;
using LeafBidAPI.App.Domain.Auction.Data;

namespace LeafBidAPI.App.Domain.Auction.Validators;

public class DeleteAuctionValidator : AbstractValidator<DeleteAuctionData>
{
    public DeleteAuctionValidator() => RuleFor(x => x.Id).GreaterThan(0);
}