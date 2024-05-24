using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models
{
    public class PendingUserProfileUpdate
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required, MaxLength(50)]
        public string? FirstName { get; set; }

        [Required, MaxLength(50)]
        public string? LastName { get; set; }

        [Required, MaxLength(20)]
        public string? Contact { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
