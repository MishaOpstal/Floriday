using FluentValidation;
using LeafBidAPI.Domain.Auction.Data;

namespace LeafBidAPI.Application.Domain.Auction.Validators;

public class UpdateAuctionValidator : AbstractValidator<UpdateAuctionData>
{
    public UpdateAuctionValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        When(x => x.Description != null, () => RuleFor(x => x.Description!).MaximumLength(1000));
        When(x => x.StartDate.HasValue, () => RuleFor(x => x.StartDate!.Value).GreaterThan(DateTime.UtcNow.AddDays(-1)));
        When(x => x.Amount.HasValue, () => RuleFor(x => x.Amount!.Value).GreaterThan(0));
        When(x => x.MinimumPrice.HasValue, () => RuleFor(x => x.MinimumPrice!.Value).GreaterThanOrEqualTo(0));
        When(x => x.AuctioneerId.HasValue, () => RuleFor(x => x.AuctioneerId!.Value).GreaterThan(0));
    }
}