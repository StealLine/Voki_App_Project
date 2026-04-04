using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.domain;
using SharedKernel.exceptions;

namespace InfrastructureShared.Base.domain_events_publisher;

internal sealed class DomainEventsPublisher(IServiceProvider serviceProvider) : IDomainEventsPublisher
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public async Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken) {
        using IServiceScope scope = serviceProvider.CreateScope();

        Type domainEventType = domainEvent.GetType();
        Type handlerType = HandlerTypeDictionary.GetOrAdd(
            domainEventType,
            et => typeof(IDomainEventHandler<>).MakeGenericType(et)
        );

        IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

        List<object> regularHandlers = [];
        IDomainToIntegrationEventsHandler? lastHandler = null;

        foreach (var handler in handlers) {
            if (handler is IDomainToIntegrationEventsHandler domainToIntegrationEventsHandler) {
                lastHandler = domainToIntegrationEventsHandler;
            }
            else if (handler is not null) {
                regularHandlers.Add(handler);
            }
        }

        foreach (var handler in regularHandlers) {
            var handlerWrapper = HandlerWrapper.Create(handler, domainEventType);
            await handlerWrapper.Handle(domainEvent, cancellationToken);
        }

        if (lastHandler is not null) {
            var wrapper = HandlerWrapper.Create(lastHandler, domainEventType);
            await wrapper.Handle(domainEvent, cancellationToken);
        }
    }

    private abstract class HandlerWrapper
    {
        public abstract Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken);

        public static HandlerWrapper Create(object handler, Type domainEventType) {
            Type wrapperType = WrapperTypeDictionary.GetOrAdd(
                domainEventType,
                et => typeof(HandlerWrapper<>).MakeGenericType(et)
            );

            var instance = Activator.CreateInstance(wrapperType, handler);
            if (instance is null || instance is not HandlerWrapper) {
                UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified(
                    $"Could not create {nameof(HandlerWrapper)} from {wrapperType} using handler of type {handler?.GetType().Name ?? "null"}"
                ));
            }

            return (HandlerWrapper)instance!;
        }
    }

    private sealed class HandlerWrapper<T>(object handler) : HandlerWrapper where T : IDomainEvent
    {
        private readonly IDomainEventHandler<T> _handler = (IDomainEventHandler<T>)handler;

        public override async Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken) {
            await _handler.Handle((T)domainEvent, cancellationToken);
        }
    }
}