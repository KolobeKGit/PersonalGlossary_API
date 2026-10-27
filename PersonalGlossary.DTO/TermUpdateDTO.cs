using System.ComponentModel.DataAnnotations;

namespace PersonalGlossary_API.Models.DTO
{
    public class TermUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public required string Title { get; set; }
        [MaxLength(150)]
        [Required]
        public required string Definition { get; set; }
    }
}
