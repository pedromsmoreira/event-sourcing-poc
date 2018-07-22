namespace EventSourcing.Infrastructure.Events
{
    using System.Threading.Tasks;

    public interface IHandleEvent<in T>
        where T : IDomainEvent
    {
        Task HandleEvent(T @event);
    }

    public interface IHandleEvent
    {
        bool CanHandle(IDomainEvent @event);
    }
}