namespace EventSourcing.WriteModel.ExceptionHandler.Middleware
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ExceptionHandlingServiceCollectionExtentions
    {
        public static IExceptionHandlingBuilder AddExceptionHandlingBuilder(this IServiceCollection services)
        {
            return new ExceptionHandlingBuilder(services);
        }

        public static IExceptionHandlingBuilder AddExceptionHandling(this IServiceCollection services, IGlobalExceptionHandler globalExceptionHandler)
        {
            var builder = services.AddExceptionHandlingBuilder();
            
            builder.Services.AddSingleton(globalExceptionHandler);

            return new ExceptionHandlingBuilder(services);
        }
    }
}