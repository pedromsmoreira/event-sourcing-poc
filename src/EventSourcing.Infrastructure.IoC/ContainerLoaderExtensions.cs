namespace EventSourcing.Infrastructure.IoC
{
    using System;

    using Application.Commands.Users;
    using Application.EventHandlers;
    using Application.Events.Users;
    using Application.Queries.Events;
    using Application.Queries.Search;
    using Application.Queries.Users;

    using Configs;

    using CorrelationId;

    using Mediator;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using Nest;

    using Persistence.ElasticSearch.Users;
    using Persistence.Mongo;
    using Persistence.Mongo.EventStreamStore;
    using Persistence.Mongo.Users;

    using Resolvers;

    public static class ContainerLoaderExtensions
    {
        public static IServiceCollection LoadPersistenceConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEventStore>(sp =>
            {
                var mongoOptions = sp.GetService<IOptions<MongoDbConfig>>();

                IEventStore eventStore;
                if (bool.Parse(configuration["toggles:enableEventStream"]))
                {
                    eventStore = new EventStreamEventStore(mongoOptions);
                }
                else
                {
                    eventStore = new MongoDbEventStore(mongoOptions);
                }

                return new LoggerEventStore(eventStore, sp.GetService<ICorrelationContextAccessor>());
            });

            services.AddSingleton<IElasticClient>(sp =>
            {
                var connectionString = configuration["App:elasticSearch:ConnectionString"];
                var port = configuration["App:elasticSearch:Port"];
                var uri = new Uri($"http://{connectionString}:{port}");

                return new ElasticClient(uri);
            });

            services.AddTransient<UsersElasticSearchRepository>();
            services.AddTransient<UsersSearchRepository>();

            return services;
        }

        public static IServiceCollection LoadApplicationConfigurations(this IServiceCollection services)
        {
            services.AddScoped<IUsersQueryHandlerAsync, UsersQueryHandlerAsync>();
            services.AddScoped<IEventsQueryHandlerAsync, EventsQueryHandlerAsync>();
            services.AddScoped<IUserCommandsProcessor, UsersCommandsProcessor>();
            services.AddScoped<ISearchQueryHandlerAsync, SearchQueryHandlerAsync>();

            return services;
        }

        public static IServiceCollection LoadEventHandlersConfigurations(this IServiceCollection services)
        {
            services.AddTransient<IDependencyResolver>(c => new AspNetCoreDependencyResolver(services));

            services.AddSingleton<IMediator, Mediator>();
            services.AddScoped<INotificationHandlerAsync<UserCreatedV1>, UserCreatedNotificationHandler>();
            services.AddScoped<INotificationHandlerAsync<UserDeletedV1>, UserDeletedNotificationHandler>();
            services.AddScoped<INotificationHandlerAsync<UserUpdatedV1>, UserUpdatedNotificationHandler>();

            return services;
        }
    }
}