using FluentValidation;
using LeafBidAPI.Domain.Auction.Data;

namespace LeafBidAPI.Application.Domain.Auction.Validators;

public class DeleteAuctionValidator : AbstractValidator<DeleteAuctionData>
{
    public DeleteAuctionValidator() => RuleFor(x => x.Id).GreaterThan(0);
}