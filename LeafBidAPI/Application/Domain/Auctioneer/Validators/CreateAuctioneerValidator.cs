using FluentValidation;
using LeafBidAPI.Domain.Auctioneer.Data;

namespace LeafBidAPI.Application.Domain.Auctioneer.Validators;

public class CreateAuctioneerValidator : AbstractValidator<CreateAuctioneerData>
{
    public CreateAuctioneerValidator() => RuleFor(x => x.UserId).GreaterThan(0);
}