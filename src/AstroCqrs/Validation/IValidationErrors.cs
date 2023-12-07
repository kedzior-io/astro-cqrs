using FluentValidation;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace AstroCqrs;

internal interface IValidationErrors<T>
{
    List<ValidationFailure> ValidationFailures { get; }

    bool ValidationFailed { get; }

    void AddError(ValidationFailure failure);

    void AddError(string message, string? errorCode = null, Severity severity = Severity.Error);

    void AddError(Expression<Func<T, object?>> property, string errorMessage, string? errorCode = null, Severity severity = Severity.Error);

    void ThrowIfAnyErrors(int? statusCode = null);

    [DoesNotReturn]
    void ThrowError(ValidationFailure failure, int? statusCode = null);

    [DoesNotReturn]
    void ThrowError(string message, int? statusCode = null);

    [DoesNotReturn]
    void ThrowError(Expression<Func<T, object?>> property, string errorMessage, int? statusCode = null);
}