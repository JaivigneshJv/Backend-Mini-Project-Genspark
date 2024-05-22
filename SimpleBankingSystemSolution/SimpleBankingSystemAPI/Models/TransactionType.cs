using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace SimpleBankingSystemAPI.Models
{
    public class TransactionType
    {
        [Key]
        public Guid TransactionTypeId { get; set; }

        [Required, MaxLength(50)]
        public string? TypeName { get; set; }

        [Required]
        public bool RaiseRequest { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
