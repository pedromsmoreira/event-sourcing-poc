namespace EventSourcing.Infrastructure.Mediator
{
    using System.Threading.Tasks;

    public interface IMediator
    {
        void Notify<TNotification>(TNotification notification) where TNotification : INotification;

        Task NotifyAsync<TNotification>(TNotification notification) where TNotification : INotification;
    }
}