namespace EventSourcing.Domain.Users
{
    using System;

    using Events;

    using EventSourcing.Infrastructure.Events;

    using Shared;

    public class User : AggregateRoot, INullObject,
        IEventApplier<UserCreated>,
        IEventApplier<UserJobChanged>,
        IEventApplier<UserNameChanged>,
        IEventApplier<UserDeleted>
    {
        public User(string name, string job)
        {
            var id = Guid.NewGuid().ToString();
            this.ApplyChange(new UserCreated(id, name, job));
        }

        public User()
        {
        }

        public string Name { get; private set; }

        public string Job { get; private set; }

        public override string Id { get; protected set; }

        public bool IsDeleted { get; private set; }

        protected override void Applier(IDomainEvent @event) => this.Apply((dynamic)@event);

        public void ChangeJob(string job)
        {
            if (string.IsNullOrWhiteSpace(job))
            {
                throw new ArgumentException();
            }

            if (!job.Equals(this.Job, StringComparison.OrdinalIgnoreCase))
            {
                this.ApplyChange(new UserJobChanged(this.Id, job));
            }
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException();
            }

            if (!name.Equals(this.Name, StringComparison.OrdinalIgnoreCase))
            {
                this.ApplyChange(new UserNameChanged(this.Id, name));
            }
        }

        public void MarkAsDeleted()
        {
            this.ApplyChange(new UserDeleted(this.Id));
        }

        public void Apply(UserCreated @event)
        {
            this.Id = @event.UserId;
            this.Name = @event.Username;
            this.Job = @event.Job;
            this.IsDeleted = @event.IsDeleted;
        }

        public void Apply(UserJobChanged @event)
        {
            this.Id = @event.UserId;
            this.Job = @event.Job;
        }

        public void Apply(UserNameChanged @event)
        {
            this.Id = @event.UserId;
            this.Name = @event.Name;
        }

        public void Apply(UserDeleted @event)
        {
            this.Id = @event.UserId;
            this.IsDeleted = @event.IsDeleted;
        }

        public virtual bool IsNull() => false;
    }
}