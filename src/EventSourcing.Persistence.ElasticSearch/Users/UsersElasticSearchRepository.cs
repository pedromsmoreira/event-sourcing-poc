namespace EventSourcing.Persistence.ElasticSearch.Users
{
    using System.Threading.Tasks;

    using Data.Model;

    using Domain.Users;

    using Elasticsearch.Net;

    using Nest;
    using Serilog;

    public class UsersElasticSearchRepository
    {
        private const string IndexName = "users";
        private const string TypeName = "userindex";
        private readonly IElasticClient client;

        public UsersElasticSearchRepository(IElasticClient client)
        {
            this.client = client;
            if (this.client.IndexExists("users").Exists)
            {
                return;
            }

            var indexSettings = new IndexSettings { NumberOfReplicas = 1, NumberOfShards = 2 };

            var indexConfig = new IndexState
            {
                Settings = indexSettings
            };

            var response = this.client.CreateIndex(
                IndexName,
                c =>
                    c.InitializeUsing(indexConfig)
                        .Mappings(m => m.Map<UserIndex>(mp => mp.AutoMap())));

            if (response.IsValid)
            {
                Log.Information("Index Created. Data: {@data}", new
                {
                    Response = response,
                    MethodName = "UsersElasticSearchRepository"
                });
            }
        }

        public async Task InsertAsync(User user)
        {
            var userIndex = UserIndexFactory.CreateSimpleIndex(user, UserAction.Create);

            var response = await this.client.IndexAsync(
                userIndex,
                i =>
                    i
                        .Index(IndexName)
                        .Type(TypeName)
                        .Id(user.Id)
                        .Refresh(Refresh.True)).ConfigureAwait(false);
        }

        public async Task UpdateAsync(User user)
        {
            var updateIndex = UserIndexFactory.CreateSimpleIndex(user, UserAction.Update);

            await this.client.UpdateAsync(
                    DocumentPath<UserIndex>
                        .Id(updateIndex.Id),
                        u => u
                            .Index(IndexName)
                            .Type(TypeName)
                        .DocAsUpsert()
                        .Doc(updateIndex))
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(User user)
        {
            await this.client.DeleteAsync<UserIndex>(
                user.Id,
                d => d
                        .Index(IndexName)
                        .Type(TypeName))
                .ConfigureAwait(false);
        }
    }
}