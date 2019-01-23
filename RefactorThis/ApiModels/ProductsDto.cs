using System.Collections.Generic;

namespace RefactorThis.ApiModels
{
    public class ProductsDto
    {
        public IEnumerable<ProductDto> Items { get; set; }
    }
}