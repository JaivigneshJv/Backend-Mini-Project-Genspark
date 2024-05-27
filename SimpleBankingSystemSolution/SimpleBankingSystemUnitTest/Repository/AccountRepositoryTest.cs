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
    public class AccountRepositoryTest
    {
        BankingContext context;
        IAccountRepository accountRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder<BankingContext> optionsBuilder = new DbContextOptionsBuilder<BankingContext>()
                                                            .UseInMemoryDatabase("dummyAccountRepositoryDB");
            context = new BankingContext(optionsBuilder.Options);
            accountRepo = new AccountRepository(context);
        }

        [Test, Order(1)]
        public async Task GetAccountsByUserIdAsync_Success()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            context.Accounts.Add(new Account
            {
                UserId = userId,
                AccountType = "Savings",
                Balance = 1000,
                CreatedDate = DateTime.Now
            });
            context.Accounts.Add(new Account
            {
                UserId = Guid.NewGuid(),
                AccountType = "Checking",
                Balance = 2000,
                CreatedDate = DateTime.Now
            });
            context.SaveChanges();

            // Act
            var result = await accountRepo.GetAccountsByUserIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test, Order(2)]
        public async Task GetAccountsByUserIdAsync_NoAccounts()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            // Act
            var result = await accountRepo.GetAccountsByUserIdAsync(userId);

            // Assert
            Assert.IsEmpty(result);
        }
    }
}
