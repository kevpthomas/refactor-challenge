using System;
using RefactorThis.Core.SharedKernel;

namespace RefactorThis.Core.Entities
{
    public class ProductOptionEntity : Entity
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}