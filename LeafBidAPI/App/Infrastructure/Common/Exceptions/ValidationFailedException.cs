using FluentResults;
using System.Text;

namespace LeafBidAPI.App.Infrastructure.Common.Exceptions;

public class ValidationFailedException(string message, IReadOnlyList<IError>? errors = null)
    : Exception(FormatMessage(message, errors))
{
    public IReadOnlyList<IError>? Errors { get; } = errors;

    private static string FormatMessage(string message, IReadOnlyList<IError>? errors)
    {
        if (errors == null || errors.Count == 0)
            return message;

        var sb = new StringBuilder(message);
        sb.AppendLine();

        foreach (var error in errors)
        {
            sb.AppendLine($"- {error.Message}");

            // Optional: show nested reasons if present
            if (!(error.Reasons?.Count > 0)) continue;

            foreach (var reason in error.Reasons)
                sb.AppendLine($"    • {reason.Message}");
        }

        return sb.ToString();
    }

    public override string ToString()
    {
        // This makes sure stack trace etc. are still shown, but with formatted errors
        return $"{Message}{Environment.NewLine}{base.ToString()}";
    }
}