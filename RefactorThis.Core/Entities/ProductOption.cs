using System;
using RefactorThis.Core.SharedKernel;

namespace RefactorThis.Core.Entities
{
    /// <summary>
    /// Entity representing a row from the ProductOption table
    /// </summary>
    public class ProductOption : Entity
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Create a <see cref="ProductOption"/> instance from id values.
        /// </summary>
        /// <param name="productId">ProductId for the associated Product record.</param>
        /// <param name="id">Id for the ProductOption record.</param>
        /// <returns>A new instance of <see cref="ProductOption"/> with its id values set.</returns>
        public static ProductOption FromId(Guid productId, Guid id)
        {
            return new ProductOption {Id = id, ProductId = productId};
        }
    }
}