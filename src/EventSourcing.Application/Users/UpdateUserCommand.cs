namespace EventSourcing.Application.Commands.Users
{
    using System;
    using Infrastructure.Commands;

    public class UpdateUserCommand : ICommand
    {
        private string name;
        private string job;

        public UpdateUserCommand(Guid id, string job, string name)
        {
            this.Id = id;
            this.Job = job;
            this.Name = name;
        }

        public string Name
        {
            get => this.name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Name cannot be null or empty");
                }

                this.name = value;
            }
        }

        public string Job
        {
            get => this.job;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Job cannot be null or empty");
                }

                this.job = value;
            }            
        }

        public Guid Id { get; private set; }
    }
}