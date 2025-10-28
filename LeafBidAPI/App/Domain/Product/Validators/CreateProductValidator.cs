using FluentValidation;
using LeafBidAPI.App.Domain.Product.Data;

namespace LeafBidAPI.App.Domain.Product.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductData>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Weight)
            .GreaterThan(0);

        RuleFor(x => x.Picture)
            .NotEmpty()
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Picture must be a valid URL.");

        RuleFor(x => x.Species)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.AuctionId)
            .GreaterThan(0);
    }
}