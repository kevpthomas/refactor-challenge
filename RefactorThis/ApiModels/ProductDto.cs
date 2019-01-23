using System;
using System.ComponentModel.DataAnnotations;

namespace RefactorThis.ApiModels
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}