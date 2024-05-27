namespace SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}

