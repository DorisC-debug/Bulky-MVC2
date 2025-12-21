using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        [Range(1, 100)]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

    }
}
