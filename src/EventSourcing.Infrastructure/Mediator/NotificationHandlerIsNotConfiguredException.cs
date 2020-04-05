namespace EventSourcing.Infrastructure.Exceptions
{
    using System;

    public class NotificationHandlerIsNotConfiguredException : Exception
    {
        public NotificationHandlerIsNotConfiguredException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotificationHandlerIsNotConfiguredException(string message)
            : base(message)
        {
        }

        public NotificationHandlerIsNotConfiguredException()
        {
        }
    }
}