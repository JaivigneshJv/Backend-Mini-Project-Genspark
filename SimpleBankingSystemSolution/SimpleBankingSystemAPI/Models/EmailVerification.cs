using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models
{
    public class EmailVerification
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string? NewEmail { get; set; }

        [Required]
        public string? VerificationCode { get; set; }

        public DateTime RequestDate { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }

}
