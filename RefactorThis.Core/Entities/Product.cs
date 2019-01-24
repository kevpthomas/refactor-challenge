using RefactorThis.Core.SharedKernel;

namespace RefactorThis.Core.Entities
{
    public class Product: Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}