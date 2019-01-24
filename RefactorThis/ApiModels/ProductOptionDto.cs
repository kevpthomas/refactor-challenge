using System;

namespace RefactorThis.ApiModels
{
    public class ProductOptionDto : ProductOptionInsertDto
    {
        public Guid ProductId { get; set; }
    }
}