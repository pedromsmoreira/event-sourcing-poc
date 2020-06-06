namespace EventSourcing.Domain.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Events;

    public abstract class AggregateRoot
    {
        private readonly List<Event> changes = new List<Event>();

        private readonly bool registeredHandlers;

        protected AggregateRoot()
        {
            if (registeredHandlers)
            {
                return;
            }

            this.registeredHandlers = true;
        }

        public abstract string Id { get; protected set; }

        public int Version { get; internal set; }

        protected Dictionary<Type, Action<IDomainEvent>> EventHandlers { get; } = new Dictionary<Type, Action<IDomainEvent>>();

        public IReadOnlyList<Event> GetUncommitedChanges()
        {
            return this.changes.AsReadOnly();
        }

        public void MarkChangesAsCommited()
        {
            this.changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<IDomainEvent> history)
        {
            foreach (var @event in history)
            {
                this.ApplyChangeEvent(@event, false);
            }
        }

        protected abstract void Applier(IDomainEvent @event);

        protected void RegisterEventHandler<TEvent>(Action<TEvent> eventHandler)
        {
            this.EventHandlers.Add(typeof(TEvent), (eh) => eventHandler((TEvent)eh));
        }

        protected void ApplyChange(IDomainEvent @event)
        {
            this.ApplyChangeEvent(@event, true);
        }

        private void ApplyChangeEvent(IDomainEvent @event, bool isNew)
        {
            this.Applier(@event);

            if (isNew)
            {
                this.changes.Add(@event as Event);
            }
        }
    }
}