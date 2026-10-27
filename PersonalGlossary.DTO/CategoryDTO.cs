using System.ComponentModel.DataAnnotations;

namespace PersonalGlossary_API.Models.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public int TermId { get; set; }
        public string? TermTitle { get; set; }

    }
}
