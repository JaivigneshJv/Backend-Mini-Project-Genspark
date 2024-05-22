using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        public byte[]? PasswordHash { get; set; }

        [Required]
        public byte[]? PasswordSalt { get; set; }

        [Required, MaxLength(50)]
        public string? FirstName { get; set; }

        [Required, MaxLength(50)]
        public string? LastName { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string? Email { get; set; }

        [Required, MaxLength(20)]
        public string? Contact { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required, MaxLength(20)]
        public string? Role { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<Account>? Accounts { get; set; }
    }
}
