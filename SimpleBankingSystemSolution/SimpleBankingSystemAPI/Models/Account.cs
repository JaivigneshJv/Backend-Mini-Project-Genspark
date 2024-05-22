using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankingSystemAPI.Models
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        [Required, ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required, MaxLength(20)]
        public string? AccountType { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        public User? User { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        public ICollection<Loan>? Loans { get; set; }
    }
}
