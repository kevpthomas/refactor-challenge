using System;
using System.ComponentModel.DataAnnotations;

namespace RefactorThis.ApiModels
{
    public class ProductOptionDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}