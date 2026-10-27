using System.ComponentModel.DataAnnotations;

namespace PersonalGlossary_API.Models.DTO
{
    public class CategoryCreateDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public int TermId { get; set; }
    }
}
