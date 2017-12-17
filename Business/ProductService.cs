using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASPNETCoreAndESearch.Models;
using Microsoft.Extensions.Configuration;
using Nest;

namespace ASPNETCoreAndESearch.Business
{
    public class ProductService : IProductService
    {
        private readonly ElasticClient _elasticClient;

        public ProductService(ConnectionSettings connectionSettings)
        {
            _elasticClient = new ElasticClient(connectionSettings);
        }

        public async Task CreateIndexAsync(string indexName)
        {
            var createIndexDescriptor = new CreateIndexDescriptor(indexName.ToLowerInvariant())
                                         .Mappings(m => m.Map<Product>(p => p.AutoMap()));

            await _elasticClient.CreateIndexAsync(createIndexDescriptor);
        }

        public async Task<bool> IndexAsync(List<Product> products, string langCode)
        {
            string indexName = $"products_{langCode}";

            IBulkResponse response = await _elasticClient.IndexManyAsync(products, indexName);

            return response.IsValid;
        }

        public async Task<ProductSearchResponse> SearchAsync(string keyword, string langCode)
        {
            ProductSearchResponse productSearchResponse = new ProductSearchResponse();
            string indexName = $"products_{langCode}";

            ISearchResponse<Product> searchResponse = await _elasticClient.SearchAsync<Product>(x => x
                .Index(indexName)
                .Query(q =>
                            q.MultiMatch(mp => mp
                                        .Query(keyword)
                                        .Fields(f => f.Fields(f1 => f1.Name, f2 => f2.Description)))
                ));

            if (searchResponse.IsValid && searchResponse.Documents != null)
            {
                productSearchResponse.Total = (int)searchResponse.Total;
                productSearchResponse.Products = searchResponse.Documents;
            }

            return productSearchResponse;
        }
    }
}