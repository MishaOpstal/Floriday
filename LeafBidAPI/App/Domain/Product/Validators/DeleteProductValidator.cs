using FluentValidation;
using LeafBidAPI.App.Domain.Product.Data;

namespace LeafBidAPI.App.Domain.Product.Validators;

public class DeleteProductValidator : AbstractValidator<DeleteProductData>
{
    public DeleteProductValidator() => RuleFor(x => x.Id).GreaterThan(0);
}