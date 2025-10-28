using FluentValidation;
using LeafBidAPI.App.Domain.Auction.Data;

namespace LeafBidAPI.App.Domain.Auction.Validators;

public class CreateAuctionValidator : AbstractValidator<CreateAuctionData>
{
    public CreateAuctionValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.StartDate).GreaterThan(DateTime.UtcNow.AddDays(-1));
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.MinimumPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ClockLocationEnum).IsInEnum();
        RuleFor(x => x.AuctioneerId).GreaterThan(0);
    }
}