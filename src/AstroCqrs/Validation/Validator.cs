using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AstroCqrs;

public abstract class Validator<TRequest> : AbstractValidator<TRequest>, IServiceResolverBase where TRequest : class
{
    public TService? TryResolve<TService>() where TService : class => Conf.ServiceResolver.TryResolve<TService>();

    public object? TryResolve(Type typeOfService) => Conf.ServiceResolver.TryResolve(typeOfService);

    public TService Resolve<TService>() where TService : class => Conf.ServiceResolver.Resolve<TService>();

    public object Resolve(Type typeOfService) => Conf.ServiceResolver.Resolve(typeOfService);

    public IServiceScope CreateScope() => Conf.ServiceResolver.CreateScope();
}