using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(
        IEnumerable<IValidator<TRequest>> validators) 
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToArray();

        if (failures.Length > 0)
        {
            var errors = failures
                .Select(f => new Error(f.PropertyName, f.ErrorMessage))
                .Distinct()
                .ToArray();

            return CreateValidationResult<TResponse>(errors);
        }

        return await next(cancellationToken);
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        var validationError = new ValidationError(errors);

        if (typeof(TResult) == typeof(Result))
        {
            return (Result.Failure(validationError) as TResult)!;
        }

        var innerType = typeof(TResult).GenericTypeArguments[0];
        var method = typeof(Result)
            .GetMethods()
            .First(m => m.Name == nameof(Result.Failure) && m.IsGenericMethod)
            .MakeGenericMethod(innerType);

        return (TResult)method.Invoke(null, [validationError])!;
    }
}
