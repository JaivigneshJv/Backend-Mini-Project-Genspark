using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankingSystemAPI.Models
{
    public class PendingAccountClosing
    {
        [Key]
        public Guid Id { get; set; }
        [Required, ForeignKey("Account")]
        public Guid AccountId { get; set; }
        [Required, MaxLength(20)]
        public string? AccountType { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        public string? Description { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public Account?Account { get; set; }
    }
}
