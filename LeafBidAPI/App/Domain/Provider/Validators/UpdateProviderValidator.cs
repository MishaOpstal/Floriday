using FluentValidation;
using LeafBidAPI.App.Domain.Provider.Data;

namespace LeafBidAPI.App.Domain.Provider.Validators;

/// <summary>
/// Validator for updating an existing provider.
/// </summary>
public class UpdateProviderValidator : AbstractValidator<UpdateProviderData>
{
    public UpdateProviderValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Provider Id must be a positive integer.");

        RuleFor(x => x.CompanyName)
            .MaximumLength(255)
            .WithMessage("Company name cannot exceed 255 characters.")
            .When(x => x.CompanyName is not null);

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be a valid positive integer.")
            .When(x => x.UserId.HasValue);
    }
}