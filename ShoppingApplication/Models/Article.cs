using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApplication.Models
{
    public class Article
    {
        public int Id { get; set; }

        [StringLength(100,MinimumLength =1)]
        [Required]
        [Display(Name="Nom")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public virtual Image? Image { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        [DataType(DataType.Currency)]
        [Range(1, 900)]
        [Required]
        [Display(Name = "Prix")]
        public decimal Price { get; set; } = decimal.One;

    }
}
