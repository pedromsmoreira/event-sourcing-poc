namespace EventSourcing.Infrastructure.Mediator
{
    using System.Threading.Tasks;

    public interface INotificationHandlerAsync<in TNotification>
    {
        Task HandleAsync(TNotification notification);
    }
}