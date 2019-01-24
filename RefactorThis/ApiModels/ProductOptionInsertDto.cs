using System;

namespace RefactorThis.ApiModels
{
    public class ProductOptionInsertDto : ProductOptionUpdateDto
    {
        public Guid Id { get; set; }
    }
}