using FluentValidation;
using LeafBidAPI.Domain.Provider.Data;

namespace LeafBidAPI.Application.Domain.Provider.Validators;

/// <summary>
/// Validator for deleting a provider.
/// </summary>
public class DeleteProviderValidator : AbstractValidator<DeleteProviderData>
{
    public DeleteProviderValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Provider Id must be a positive integer.");
    }
}