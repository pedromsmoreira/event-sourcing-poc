namespace EventSourcing.WriteModel.ExceptionHandler.Middleware
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class ExceptionHandlingBuilder : IExceptionHandlingBuilder
    {
        public ExceptionHandlingBuilder(IServiceCollection services)
        {
            this.Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; set; }
    }
}