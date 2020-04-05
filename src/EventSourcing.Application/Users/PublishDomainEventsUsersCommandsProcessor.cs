namespace EventSourcing.Application.Commands.Users
{
    using System;
    using System.Threading.Tasks;

    public class PublishDomainEventsUsersCommandsProcessor : IUserCommandsProcessor
    {
        public Task<Guid> ProcessAsync(CreateUserCommand command) => throw new NotImplementedException();

        public Task<Guid> ProcessAsync(UpdateUserCommand command) => throw new NotImplementedException();

        public Task ProcessAsync(DeleteUserCommand command) => throw new NotImplementedException();
    }
}