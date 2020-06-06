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
                // validation should be placed in controller
                if (string.IsNullOrWhiteSpace(value))
                {
                    // register event failure instead of Exception
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
                // validation should be placed in controller
                if (string.IsNullOrWhiteSpace(value))
                {
                    // register event failure instead of Exception
                    throw new ArgumentException("Job cannot be null or empty");
                }

                this.job = value;
            }
        }

        public Guid Id { get; private set; }
    }
}