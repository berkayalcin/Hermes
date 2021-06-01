using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace Hermes.API.Advertisement.Domain.Services.ElasticSearch
{
    public interface IElasticSearchService
    {
        IElasticClient Client { get; }
        Task<ISearchResponse<T>> Search<T>(SearchRequest searchRequest) where T : class;
        Task CreateIndex<T>(string indexName) where T : class;
        Task BulkAddOrUpdate<T>(IEnumerable<T> data, string indexName) where T : class;
        Task AddOrUpdate<T>(T data, string indexName) where T : class;
        Task CreateIndexIfNotExists<T>(string indexName) where T : class;
        Task Remove(string indexName);

        Task<ISearchResponse<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> searchDescriptor)
            where T : class;

        Task Purge<T>(string indexName) where T : class;
    }
}