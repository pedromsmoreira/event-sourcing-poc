namespace EventSourcing.Infrastructure.Mediator
{
    public interface INotificationHandler<in TNotification>
        where TNotification : INotification
    {
        void Handle(TNotification notification);
    }
}