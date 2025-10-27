using FluentValidation;
using LeafBidAPI.Domain.Product.Data;

namespace LeafBidAPI.Application.Domain.Product.Validators;

public class GetProductValidator : AbstractValidator<GetProductData>
{
    public GetProductValidator() => RuleFor(x => x.Id).GreaterThan(0);
}