namespace EventSourcing.Domain.Users
{
    using EventSourcing.Infrastructure.Events;

    internal interface IEventApplier<TEvent> where TEvent : IDomainEvent
    {
        void Apply(TEvent @event);
    }
}