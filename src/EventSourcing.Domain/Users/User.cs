namespace EventSourcing.Domain.Users
{
    using System;

    using Events;
    using EventSourcing.Infrastructure.Events;
    using Shared;

    // https://stackoverflow.com/questions/9759141/overloading-in-java-and-multiple-dispatch

    internal interface IEventApplier<TEvent> where TEvent : IDomainEvent
    {
        void Apply(TEvent @event);
    }

    internal interface IUserCreatedApplier : IEventApplier<UserCreated>
    {
    }

    public class User : AggregateRoot, INullObject,
        IUserCreatedApplier,
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

        protected override void RegisterEventHandlers()
        {
            this.RegisterEventHandler<UserCreated>(this.ProcessUserCreated);
            this.RegisterEventHandler<UserJobChanged>(this.ProcessUserJobChanged);
            this.RegisterEventHandler<UserNameChanged>(this.ProcessUserNameChanged);
            this.RegisterEventHandler<UserDeleted>(this.ProcessUserDeleted);
        }

        protected override void Applier(IDomainEvent @event) => this.Apply((dynamic)@event);

        public void ChangeJob(string job)
        {
            if (string.IsNullOrWhiteSpace(job))
            {
                throw new ArgumentException();
            }

            this.ApplyChange(new UserJobChanged(this.Id, job));
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException();
            }

            this.ApplyChange(new UserNameChanged(this.Id, name));
        }

        public void MarkAsDeleted()
        {
            this.ApplyChange(new UserDeleted(this.Id));
        }

        protected void ProcessUserCreated(UserCreated @event)
        {
            this.Id = @event.UserId;
            this.Name = @event.Username;
            this.Job = @event.Job;
            this.IsDeleted = @event.IsDeleted;
        }

        public void Apply(UserCreated @event)
        {
            this.Id = @event.UserId;
            this.Name = @event.Username;
            this.Job = @event.Job;
            this.IsDeleted = @event.IsDeleted;
        }

        protected void ProcessUserJobChanged(UserJobChanged @event)
        {
            this.Id = @event.UserId;
            this.Job = @event.Job;
        }

        public void Apply(UserJobChanged @event)
        {
            this.Id = @event.UserId;
            this.Job = @event.Job;
        }

        protected void ProcessUserNameChanged(UserNameChanged @event)
        {
            this.Id = @event.UserId;
            this.Name = @event.Name;
        }

        public void Apply(UserNameChanged @event)
        {
            this.Id = @event.UserId;
            this.Name = @event.Name;
        }

        protected void ProcessUserDeleted(UserDeleted @event)
        {
            this.Id = @event.UserId;
            this.IsDeleted = @event.IsDeleted;
        }

        public void Apply(UserDeleted @event)
        {
            this.Id = @event.UserId;
            this.IsDeleted = @event.IsDeleted;
        }

        public virtual bool IsNull() => false;
    }
}