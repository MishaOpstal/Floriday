using FluentValidation;
using LeafBidAPI.App.Domain.Provider.Data;

namespace LeafBidAPI.App.Domain.Provider.Validators;

/// <summary>
/// Validator for fetching a provider.
/// </summary>
public class GetProviderValidator : AbstractValidator<GetProviderData>
{
    public GetProviderValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Provider Id must be a positive integer.");
    }
}