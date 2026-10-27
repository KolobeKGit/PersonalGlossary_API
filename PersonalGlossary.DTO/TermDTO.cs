using System.ComponentModel.DataAnnotations;

namespace PersonalGlossary_API.Models.DTO
{
    public class TermDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Definition { get; set; }

    }
}
