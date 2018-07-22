namespace EventSourcing.Domain.Users.Events
{
    using Shared;

    public class UserNameChanged : Event
    {
        public UserNameChanged(string userId, string name)
        {
            this.UserId = userId;
            this.Name = name;
        }

        public string UserId { get; set; }

        public string Name { get; set; }
    }
}