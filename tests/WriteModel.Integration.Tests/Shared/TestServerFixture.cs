namespace WriteModel.Integration.Tests.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Data;
    using Data.EventStreamStore;
    using EventSourcing.Infrastructure.Configs;
    using EventSourcing.WriteModel;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;

    public class TestServerFixture : IDisposable
    {
        private IConfiguration configuration;
        private readonly TestServer server;

        public TestServerFixture()
        {
            this.server = new TestServer(
                WebHost
                    .CreateDefaultBuilder()
                    .ConfigureAppConfiguration((hostBuilder, configBuilder) =>
                    {
                        // HAMMERTIME in dev appsettings
                        configBuilder
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();

                        configuration = configBuilder.Build();
                    })
                    .UseStartup<Startup>());

            this.Client = this.server.CreateClient();

            var config = new MongoDbConfig
            {
                User = this.configuration["App:mongodb:User"],
                Password = this.configuration["App:mongodb:Password"],
                Port = int.Parse(this.configuration["App:mongodb:Port"]),
                ConnectionString = this.configuration["App:mongodb:ConnectionString"],
                DatabaseName = this.configuration["App:mongodb:DatabaseName"],
            };

            this.EventStreamEventStore = new EventStreamEventStore(config);
            this.Database = new MongoDbEventStore(config);
            this.IdsToDelete = new List<Guid>();
        }

        public MongoDbEventStore Database { get; }

        public HttpClient Client { get; set; }

        public EventStreamEventStore EventStreamEventStore { get; }

        public List<Guid> IdsToDelete { get; }

        public void Dispose()
        {
            var ids = this.IdsToDelete.Distinct();
            foreach (var id in ids)
            {
                this.EventStreamEventStore.DeleteEventStreams(id).Wait();
                this.Database.DeleteEvents(id).Wait();
            }
        }
    }
}