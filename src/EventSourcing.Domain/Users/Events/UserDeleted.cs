namespace EventSourcing.Domain.Users.Events
{
    using Shared;

    public class UserDeleted : Event
    {
        public UserDeleted(string userId)
        {
            this.UserId = userId;
            this.IsDeleted = true;
        }

        public bool IsDeleted { get; set; }

        public string UserId { get; set; }
    }
}