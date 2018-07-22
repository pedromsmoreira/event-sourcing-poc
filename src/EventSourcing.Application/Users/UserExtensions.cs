namespace EventSourcing.Application.Commands.Users
{
    using Domain.Users;

    public static class UserExtensions
    {
        public static void ApplyUpdate(this User currentUser, UpdateUserCommand command)
        {
            if (!currentUser.Name.Equals(command.Name))
            {
                currentUser.ChangeName(command.Name);
            }

            if (!currentUser.Job.Equals(command.Job))
            {
                currentUser.ChangeJob(command.Job);
            }
        }
    }
}