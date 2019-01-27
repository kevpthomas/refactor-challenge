using System;
using RefactorThis.Core.SharedKernel;

namespace RefactorThis.Core.Entities
{
    /// <summary>
    /// Entity representing a row from the Product table
    /// </summary>
    public class Product: Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        /// <summary>
        /// Create a <see cref="Product"/> instance from an id value.
        /// </summary>
        /// <param name="id">Id for the Product record.</param>
        /// <returns>A new instance of <see cref="Product"/> with its id value set.</returns>
        public static Product FromId(Guid id)
        {
            return new Product {Id = id};
        }
    }
}