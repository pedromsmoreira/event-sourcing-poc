namespace EventSourcing.Application.Commands.Users
{
    using System;
    using Infrastructure.Commands;

    public class CreateUserCommand : ICommand
    {
        public CreateUserCommand(string name, string job)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Job = job;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Job { get; }
    }
}