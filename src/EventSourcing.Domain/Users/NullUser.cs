namespace EventSourcing.Domain.Users
{
    public class NullUser : User
    {
        public override bool IsNull() => true;
    }
}