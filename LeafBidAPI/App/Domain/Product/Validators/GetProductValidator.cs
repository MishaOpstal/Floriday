using FluentValidation;
using LeafBidAPI.App.Domain.Product.Data;

namespace LeafBidAPI.App.Domain.Product.Validators;

public class GetProductValidator : AbstractValidator<GetProductData>
{
    public GetProductValidator() => RuleFor(x => x.Id).GreaterThan(0);
}