namespace EventSourcing.Infrastructure.Queries
{
    using System.Threading.Tasks;

    public interface IQueryHandlerAsync<in TQuery, TResult>
        where TQuery : IQuery
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}