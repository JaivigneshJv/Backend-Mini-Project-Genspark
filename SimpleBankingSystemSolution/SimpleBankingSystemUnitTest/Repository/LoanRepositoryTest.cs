using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystemUnitTest.Repository
{
    public class LoanRepositoryTest
    {
        BankingContext context;
        ILoanRepository loanRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder<BankingContext> optionsBuilder = new DbContextOptionsBuilder<BankingContext>()
                                                            .UseInMemoryDatabase("dummyLoanRepositoryDB");
            context = new BankingContext(optionsBuilder.Options);
            loanRepo = new LoanRepository(context);
        }

        [Test, Order(1)]
        public async Task GetLoansByAccountId_Success()
        {
            // Arrange
            Guid accountId = Guid.NewGuid();
            context.Loans.Add(new Loan
            {
                AccountId = accountId,
                Amount = 1000,
                PendingAmount = 500,
                Status = "Opened",
                AppliedDate = DateTime.Now,
                TargetDate = DateTime.Now.AddDays(30)
            });
            context.Loans.Add(new Loan
            {
                AccountId = Guid.NewGuid(),
                Amount = 2000,
                PendingAmount = 1500,
                Status = "Opened",
                AppliedDate = DateTime.Now,
                TargetDate = DateTime.Now.AddDays(60)
            });
            context.SaveChanges();

            // Act
            var result = await loanRepo.GetLoansByAccountId(accountId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test, Order(2)]
        public async Task GetAllPendingLoans_Success()
        {
            // Arrange
            context.Loans.Add(new Loan
            {
                AccountId = Guid.NewGuid(),
                Amount = 1000,
                PendingAmount = 500,
                Status = "Pending",
                AppliedDate = DateTime.Now,
                TargetDate = DateTime.Now.AddDays(30)
            });
            context.Loans.Add(new Loan
            {
                AccountId = Guid.NewGuid(),
                Amount = 2000,
                PendingAmount = 1500,
                Status = "Opened",
                AppliedDate = DateTime.Now,
                TargetDate = DateTime.Now.AddDays(60)
            });
            context.SaveChanges();

            // Act
            var result = await loanRepo.GetAllPendingLoans();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test, Order(3)]
        public async Task GetAllOpenedLoans_Success()
        {
            // Arrange
            context.Loans.Add(new Loan
            {
                AccountId = Guid.NewGuid(),
                Amount = 1000,
                PendingAmount = 500,
                Status = "Opened",
                AppliedDate = DateTime.Now,
                TargetDate = DateTime.Now.AddDays(30)
            });
            context.Loans.Add(new Loan
            {
                AccountId = Guid.NewGuid(),
                Amount = 2000,
                PendingAmount = 1500,
                Status = "Pending",
                AppliedDate = DateTime.Now,
                TargetDate = DateTime.Now.AddDays(60)
            });
            context.SaveChanges();

            // Act
            var result = await loanRepo.GetAllOpenedLoans();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());
        }

        [Test, Order(4)]
        public async Task GetAllClosedLoans_Success()
        {
            // Arrange
            context.Loans.Add(new Loan
            {
                AccountId = Guid.NewGuid(),
                Amount = 1000,
                PendingAmount = 0,
                Status = "Closed",
                AppliedDate = DateTime.Now.AddDays(-90),
                TargetDate = DateTime.Now.AddDays(-60),
                RepaidDate = DateTime.Now.AddDays(-30)
            });
            context.Loans.Add(new Loan
            {
                AccountId = Guid.NewGuid(),
                Amount = 2000,
                PendingAmount = 500,
                Status = "Opened",
                AppliedDate = DateTime.Now.AddDays(-90),
                TargetDate = DateTime.Now.AddDays(-60)
            });
            context.SaveChanges();

            // Act
            var result = await loanRepo.GetAllClosedLoans();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test, Order(5)]
        public async Task GetAllRejectedLoans_Success()
        {
            // Arrange
            context.Loans.Add(new Loan
            {
                AccountId = Guid.NewGuid(),
                Amount = 1000,
                PendingAmount = 500,
                Status = "Rejected",
                AppliedDate = DateTime.Now.AddDays(-90),
                TargetDate = DateTime.Now.AddDays(-60)
            });
            context.Loans.Add(new Loan
            {
                AccountId = Guid.NewGuid(),
                Amount = 2000,
                PendingAmount = 1500,
                Status = "Opened",
                AppliedDate = DateTime.Now.AddDays(-90),
                TargetDate = DateTime.Now.AddDays(-60)
            });
            context.SaveChanges();

            // Act
            var result = await loanRepo.GetAllRejectedLoans();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}
