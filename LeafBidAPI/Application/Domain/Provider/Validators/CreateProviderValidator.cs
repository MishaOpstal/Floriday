using FluentValidation;
using LeafBidAPI.Domain.Provider.Data;

namespace LeafBidAPI.Application.Domain.Provider.Validators;

/// <summary>
/// Validator for creating a new provider.
/// </summary>
public class CreateProviderValidator : AbstractValidator<CreateProviderData>
{
    public CreateProviderValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be a valid positive integer.");

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage("Company name is required.")
            .MaximumLength(255)
            .WithMessage("Company name cannot exceed 255 characters.");
    }
}