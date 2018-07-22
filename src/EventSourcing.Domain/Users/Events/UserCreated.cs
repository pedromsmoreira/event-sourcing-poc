namespace EventSourcing.Domain.Users.Events
{
    using Shared;

    public class UserCreated : Event
    {
        public UserCreated(string userId, string username, string job)
        {
            this.UserId = userId;
            this.Username = username;
            this.Job = job;
            this.IsDeleted = false;
        }

        public bool IsDeleted { get; set; }

        public string Username { get; set; }

        public string Job { get; set; }

        public string UserId { get; set; }
    }
}