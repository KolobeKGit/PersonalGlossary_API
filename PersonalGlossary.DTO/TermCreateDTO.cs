using System.ComponentModel.DataAnnotations;

namespace PersonalGlossary_API.Models.DTO
{
    public class TermCreateDTO
    {
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Definition { get; set; }
    }
}
