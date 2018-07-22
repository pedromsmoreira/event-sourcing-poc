namespace EventSourcing.Infrastructure.Events
{
    public interface IDomainEvent
    {
        long Timestamp { get; set; }
    }
}