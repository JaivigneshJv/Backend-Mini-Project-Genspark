using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Models;
using System.Security.Cryptography;

namespace SimpleBankingSystemAPI.Contexts
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options) : base(options) { }
        public DbSet<User>? Users { get; set; }
        public DbSet<Account>? Accounts { get; set; }
        public DbSet<Transaction>? Transactions { get; set; }
        public DbSet<TransactionType>? TransactionTypes { get; set; }
        public DbSet<Loan>? Loans { get; set; }
        public DbSet<LoanRepayment>? LoanRepayments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Receiver)
                .WithMany()
                .HasForeignKey(t => t.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.TransactionType)
                .WithMany(tt => tt.Transactions)
                .HasForeignKey(t => t.TransactionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Account)
                .WithMany(a => a.Loans)
                .HasForeignKey(l => l.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoanRepayment>()
                .HasOne(lr => lr.Loan)
                .WithMany(l => l.LoanRepayments)
                .HasForeignKey(lr => lr.LoanId)
                .OnDelete(DeleteBehavior.Restrict);


            var hmac = new HMACSHA512();
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "Admin",
                PasswordHash = hmac.Key,
                PasswordSalt = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("password")),
                FirstName = "Admin",
                LastName = "User",
                Email = "adminuser@simplebank.com",
                Contact = "1234567890",
                DateOfBirth = new DateTime(2002, 10, 11),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Role = "Admin",
                IsActive = true

            };
            modelBuilder.Entity<User>().HasData(adminUser);
        }
        
    }
}
