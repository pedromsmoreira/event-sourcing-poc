namespace EventSourcing.Infrastructure.Mediator
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Exceptions;

    using Resolvers;

    public class Mediator : IMediator
    {
        private readonly IDependencyResolver dependencyResolver;

        public Mediator(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        public void Notify<TNotification>(TNotification notification) where TNotification : INotification
        {
            IEnumerable<INotificationHandler<TNotification>> handlers;
            try
            {
                handlers = this.dependencyResolver.GetInstance<INotificationHandler<TNotification>>();
            }
            catch (Exception exception)
            {
                throw new NotificationHandlerIsNotConfiguredException($"Handler for Command: {typeof(TNotification)} is not configured.", exception);
            }

            foreach (var handler in handlers)
            {
                handler.Handle(notification);
            }
        }

        public async Task NotifyAsync<TNotification>(TNotification notification) where TNotification : INotification
        {
            IEnumerable<INotificationHandlerAsync<TNotification>> handlers;

            try
            {
                handlers = this.dependencyResolver.GetInstance<INotificationHandlerAsync<TNotification>>();
            }
            catch (Exception exception)
            {
                throw new NotificationHandlerIsNotConfiguredException($"Handler for Command: {typeof(TNotification)} is not configured.", exception);
            }

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(notification).ConfigureAwait(false);
            }
        }
    }
}