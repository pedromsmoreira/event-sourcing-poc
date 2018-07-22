namespace EventSourcing.Infrastructure.Commands
{
    using System.Threading.Tasks;

    public interface IProcess<in TCommand, TOutput>
        where TCommand : ICommand
    {
        Task<TOutput> ProcessAsync(TCommand command);
    }

    public interface IProcess<in TCommand>
        where TCommand : ICommand
    {
        Task ProcessAsync(TCommand command);
    }
}