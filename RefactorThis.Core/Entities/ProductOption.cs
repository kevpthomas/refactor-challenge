using System;
using RefactorThis.Core.SharedKernel;

namespace RefactorThis.Core.Entities
{
    public class ProductOption : Entity
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }


        public static ProductOption FromId(Guid productId, Guid id)
        {
            return new ProductOption {Id = id, ProductId = productId};
        }
    }
}