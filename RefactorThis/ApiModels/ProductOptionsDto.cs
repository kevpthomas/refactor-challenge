using System.Collections.Generic;

namespace RefactorThis.ApiModels
{
    public class ProductOptionsDto
    {
        public IEnumerable<ProductOptionDto> Items { get; set; }
    }
}