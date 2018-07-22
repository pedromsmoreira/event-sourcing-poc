namespace EventSourcing.Application.Commands.Users
{
    using System;
    using Infrastructure.Commands;

    public interface IUserCommandsProcessor :
        IProcess<CreateUserCommand, Guid>,
        IProcess<UpdateUserCommand, Guid>,
        IProcess<DeleteUserCommand>
    {
    }
}