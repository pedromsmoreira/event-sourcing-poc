namespace EventSourcing.WriteModel.ExceptionHandler.Middleware
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IExceptionHandlingBuilder
    {
        IServiceCollection Services { get; set; }
    }
}