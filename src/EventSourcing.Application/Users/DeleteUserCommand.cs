namespace EventSourcing.Application.Commands.Users
{
    using System;

    using Infrastructure.Commands;

    public class DeleteUserCommand : ICommand
    {
        public DeleteUserCommand(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}