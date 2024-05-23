namespace SimpleBankingSystemAPI.Models.DTOs.UserDTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
    }
}
