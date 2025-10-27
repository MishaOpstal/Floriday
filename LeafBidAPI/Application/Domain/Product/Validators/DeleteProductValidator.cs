using FluentValidation;
using LeafBidAPI.Domain.Product.Data;

namespace LeafBidAPI.Application.Domain.Product.Validators;

public class DeleteProductValidator : AbstractValidator<DeleteProductData>
{
    public DeleteProductValidator() => RuleFor(x => x.Id).GreaterThan(0);
}