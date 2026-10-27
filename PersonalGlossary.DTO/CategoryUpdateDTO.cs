using System.ComponentModel.DataAnnotations;

namespace PersonalGlossary_API.Models.DTO
{
    public class CategoryUpdateDTO
    {
        [Required]  
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }
        [Required]
        public int TermId { get; set; }
    }
}
