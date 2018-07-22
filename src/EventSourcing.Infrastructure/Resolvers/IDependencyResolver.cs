namespace EventSourcing.Infrastructure.Resolvers
{
    using System;
    using System.Collections.Generic;

    public interface IDependencyResolver
    {
        object GetInstance(Type type);

        IEnumerable<T> GetInstance<T>();
    }
}