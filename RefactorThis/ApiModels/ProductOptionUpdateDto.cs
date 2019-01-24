using System.ComponentModel.DataAnnotations;

namespace RefactorThis.ApiModels
{
    public class ProductOptionUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}