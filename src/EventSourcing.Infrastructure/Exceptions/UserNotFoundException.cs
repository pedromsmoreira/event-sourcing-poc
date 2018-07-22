namespace EventSourcing.Infrastructure.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string message)
            : base(message)
        {
        }
    }
}