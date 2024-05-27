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
    public class PendingAccountTransactionRepositoryTest
    {
        BankingContext context;
        IPendingAccountTransactionRepository pendingAccountTransactionRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder<BankingContext> optionsBuilder = new DbContextOptionsBuilder<BankingContext>()
                                                            .UseInMemoryDatabase("dummyPendingAccountTransactionRepositoryDB");
            context = new BankingContext(optionsBuilder.Options);
            pendingAccountTransactionRepo = new PendingAccountTransactionRepository(context);
        }

        [Test, Order(1)]
        public async Task GetAllTransactionByAccountId_Success()
        {
            // Arrange
            Guid accountId = Guid.NewGuid();
            context.PendingAccountTransactions.Add(new PendingAccountTransaction
            {
                AccountId = accountId,
                ReceiverId = Guid.NewGuid(),
                Amount = 100,
                TransactionType = "Test",
                Timestamp = DateTime.Now,
                IsApproved = true,
                IsRejected = false
            });
            context.PendingAccountTransactions.Add(new PendingAccountTransaction
            {
                AccountId = accountId,
                ReceiverId = Guid.NewGuid(),
                Amount = 200,
                TransactionType = "Test",
                Timestamp = DateTime.Now,
                IsApproved = false,
                IsRejected = false
            });
            context.SaveChanges();

            // Act
            var result = await pendingAccountTransactionRepo.GetAllTransactionByAccountId(accountId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test, Order(2)]
        public async Task GetAllPendingTransactions_Success()
        {
            // Arrange
            context.PendingAccountTransactions.Add(new PendingAccountTransaction
            {
                AccountId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Amount = 100,
                TransactionType = "Test",
                Timestamp = DateTime.Now,
                IsApproved = false,
                IsRejected = false
            });
            context.PendingAccountTransactions.Add(new PendingAccountTransaction
            {
                AccountId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Amount = 200,
                TransactionType = "Test",
                Timestamp = DateTime.Now,
                IsApproved = false,
                IsRejected = true
            });
            context.SaveChanges();

            // Act
            var result = await pendingAccountTransactionRepo.GetAllPendingTransactions();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test, Order(3)]
        public async Task GetAllAcceptedTransactions_Success()
        {
            // Arrange
            context.PendingAccountTransactions.Add(new PendingAccountTransaction
            {
                AccountId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Amount = 100,
                TransactionType = "Test",
                Timestamp = DateTime.Now,
                IsApproved = true,
                IsRejected = false
            });
            context.PendingAccountTransactions.Add(new PendingAccountTransaction
            {
                AccountId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Amount = 200,
                TransactionType = "Test",
                Timestamp = DateTime.Now,
                IsApproved = false,
                IsRejected = false
            });
            context.SaveChanges();

            // Act
            var result = await pendingAccountTransactionRepo.GetAllAcceptedTransactions();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test, Order(4)]
        public async Task GetAllRejectedTransactions_Success()
        {
            // Arrange
            context.PendingAccountTransactions.Add(new PendingAccountTransaction
            {
                AccountId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Amount = 100,
                TransactionType = "Test",
                Timestamp = DateTime.Now,
                IsApproved = false,
                IsRejected = true
            });
            context.PendingAccountTransactions.Add(new PendingAccountTransaction
            {
                AccountId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Amount = 200,
                TransactionType = "Test",
                Timestamp = DateTime.Now,
                IsApproved = false,
                IsRejected = false
            });
            context.SaveChanges();

            // Act
            var result = await pendingAccountTransactionRepo.GetAllRejectedTransactions();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
    }
}
