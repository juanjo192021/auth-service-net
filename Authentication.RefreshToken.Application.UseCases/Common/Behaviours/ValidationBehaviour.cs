using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using FluentValidation;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .GroupBy(f => ToCamelCase(f.PropertyName))
                .Select(g => new BaseError()
                {
                    PropertyMessage = g.Key,
                    ErrorMessage = g
                    .Select(e => e.ErrorMessage)
                    .Where(msg => !string.IsNullOrEmpty(msg))
                    .Distinct()
                    .ToList()
                })
                .Where(e => e.ErrorMessage!.Any())
                .ToList();

            if (failures.Count > 0)
            {
                throw new ValidationExceptionCustom(failures);
            }

            return await next();
        }

        private static string ToCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value) || char.IsLower(value[0]))
                return value;

            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}
