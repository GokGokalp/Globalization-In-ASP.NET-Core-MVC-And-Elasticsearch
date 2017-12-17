using System.Collections.Generic;

namespace ASPNETCoreAndESearch.Models
{
    public class ProductSearchResponse
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public IEnumerable<Product> Products { get; set; }
        
    }
}