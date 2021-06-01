using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace Hermes.API.Advertisement.Domain.Services.ElasticSearch
{
    public class ElasticSearchService : IElasticSearchService
    {
        public ElasticSearchService(IElasticClient client)
        {
            Client = client;
        }

        public IElasticClient Client { get; }

        public async Task<ISearchResponse<T>> Search<T>(SearchRequest searchRequest) where T : class
        {
            var result = Client.Search<T>(searchRequest);
            if (!result.IsValid)
                throw new OperationCanceledException(result.ServerError.ToString());
            return result;
        }

        public async Task CreateIndex<T>(string indexName) where T : class
        {
            var createIndexResponse = await Client.Indices
                .CreateAsync(indexName, s => s
                    .Settings(ss =>
                        ss
                            .NumberOfReplicas(0)
                            .NumberOfShards(1)
                    )
                    .Map(descriptor => descriptor.AutoMap<T>()));

            if (!createIndexResponse.IsValid)
            {
                throw new OperationCanceledException(createIndexResponse.ServerError.ToString());
            }
        }

        public async Task BulkAddOrUpdate<T>(IEnumerable<T> data, string indexName) where T : class
        {
            await CreateIndexIfNotExists<T>(indexName);

            Client.BulkAll(data, b =>
                b
                    .Index(indexName)
                    .BackOffTime("30s")
                    .BackOffRetries(2)
                    .RefreshOnCompleted()
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    .Size(100)
            ).Wait(TimeSpan.FromMinutes(15), next => { });
        }

        public async Task AddOrUpdate<T>(T data, string indexName) where T : class
        {
            await CreateIndexIfNotExists<T>(indexName);

            var indexResponse = await Client.IndexAsync(data, i => i.Index(indexName));
            if (!indexResponse.IsValid)
                throw new OperationCanceledException(indexResponse.ServerError.ToString());
        }

        public async Task CreateIndexIfNotExists<T>(string indexName) where T : class
        {
            if ((await Client.Indices.ExistsAsync(indexName)).Exists == false)
            {
                await CreateIndex<T>(indexName);
            }
        }

        public async Task Purge<T>(string indexName) where T : class
        {
            await Client.DeleteByQueryAsync<T>(del => del
                .Index(indexName)
                .Query(q => q.QueryString(qs => qs.Query("*")))
            );
        }

        public async Task Remove(string indexName)
        {
            await Client.Indices.DeleteAsync(indexName);
        }

        public async Task<ISearchResponse<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> searchDescriptor)
            where T : class
        {
            var searchResponse = await Client.SearchAsync<T>(searchDescriptor);
            return searchResponse;
        }
    }
}