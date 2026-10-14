namespace PersonalGlossary_API.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
