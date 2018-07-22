namespace EventSourcing.Persistence.Mongo
{
    public interface IUuid<T>
    {
        T Id { get; set; }
    }
}