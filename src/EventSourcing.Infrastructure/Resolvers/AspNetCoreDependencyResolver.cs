namespace EventSourcing.Infrastructure.Resolvers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Extensions.DependencyInjection;

    public class AspNetCoreDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider serviceProvider;

        public AspNetCoreDependencyResolver(IServiceCollection services)
        {
            this.serviceProvider = services.BuildServiceProvider();
        }

        public object GetInstance(Type type)
        {
            return this.serviceProvider.GetService(type);
        }

        public IEnumerable<T> GetInstance<T>()
        {
            return this.serviceProvider.GetServices<T>();
        }
    }
}