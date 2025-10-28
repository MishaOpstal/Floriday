using FluentValidation;
using LeafBidAPI.App.Domain.Product.Data;

namespace LeafBidAPI.App.Domain.Product.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductData>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        When(x => x.Name is not null, () => RuleFor(x => x.Name!).MaximumLength(255));
        When(x => x.Weight.HasValue, () => RuleFor(x => x.Weight!.Value).GreaterThan(0));
        When(x => x.Picture is not null, () => 
            RuleFor(x => x.Picture!).Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Picture must be a valid URL."));
        When(x => x.Species is not null, () => RuleFor(x => x.Species!).MaximumLength(255));
        When(x => x.Stock.HasValue, () => RuleFor(x => x.Stock!.Value).GreaterThanOrEqualTo(0));
        When(x => x.AuctionId.HasValue, () => RuleFor(x => x.AuctionId!.Value).GreaterThan(0));
    }
}