namespace EventSourcing.Domain.Users.Events
{
    using Shared;

    public class UserJobChanged : Event
    {
        public UserJobChanged(string userId, string job)
        {
            this.UserId = userId;
            this.Job = job;
        }

        public string UserId { get; set; }

        public string Job { get; set; }
    }
}