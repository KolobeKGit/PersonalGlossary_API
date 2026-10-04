using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PersonalGlossary_API.Models
{
    public class Term
    {
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Definition { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
