namespace EventSourcing.Infrastructure.Exceptions
{
    public class AggregateNotFoundException : NotFoundException
    {
        public AggregateNotFoundException()
            : base("aggregate Id was not found.")
        {
        }

        public AggregateNotFoundException(string message)
            : base(message)
        {
        }
    }
}