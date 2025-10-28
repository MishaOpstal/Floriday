using FluentResults;
using FluentValidation;

namespace LeafBidAPI.App.Infrastructure.Common.Repositories;

public abstract class BaseRepository
{
    protected async static Task<Result> ValidateAsync<T>(IValidator<T> validator, T data)
    {
        var result = await validator.ValidateAsync(data);
        return result.IsValid
            ? Result.Ok()
            : Result.Fail(result.Errors.Select(e => e.ErrorMessage));
    }
}