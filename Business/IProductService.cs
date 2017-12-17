using System.Collections.Generic;
using System.Threading.Tasks;
using ASPNETCoreAndESearch.Models;

namespace ASPNETCoreAndESearch.Business
{
    public interface IProductService
    {
        Task CreateIndexAsync(string indexName);
        Task<bool> IndexAsync(List<Product> products, string langCode);
        Task<ProductSearchResponse> SearchAsync(string keyword, string langCode);
    }
}