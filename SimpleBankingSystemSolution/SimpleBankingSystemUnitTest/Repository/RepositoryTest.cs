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
    public class RepositoryTest
    {
        private BankingContext _context;
        private IRepository<Guid, Transaction> _transactionRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder<BankingContext> optionsBuilder = new DbContextOptionsBuilder<BankingContext>()
                .UseInMemoryDatabase("dummyRepositoryDB");
            _context = new BankingContext(optionsBuilder.Options);
            _transactionRepo = new Repository<Guid, Transaction>(_context);
        }

        [Test, Order(1)]
        public async Task AddTransaction_Success()
        {
            // Arrange
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Amount = 100,
                TransactionType = "Deposit",
                Timestamp = DateTime.Now,
                IsRecurring = false
            };

            // Act
            var addedTransaction = await _transactionRepo.Add(transaction);

            // Assert
            Assert.IsNotNull(addedTransaction);
            Assert.AreEqual(transaction.Id, addedTransaction.Id);
        }

        [Test, Order(2)]
        public async Task GetByIdTransaction_Success()
        {
            // Arrange
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Amount = 100,
                TransactionType = "Deposit",
                Timestamp = DateTime.Now,
                IsRecurring = false
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            // Act
            var retrievedTransaction = await _transactionRepo.GetById(transaction.Id);

            // Assert
            Assert.IsNotNull(retrievedTransaction);
            Assert.AreEqual(transaction.Id, retrievedTransaction.Id);
        }

        [Test, Order(3)]
        public async Task UpdateTransaction_Success()
        {
            // Arrange
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Amount = 100,
                TransactionType = "Deposit",
                Timestamp = DateTime.Now,
                IsRecurring = false
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            transaction.Amount = 200;
            transaction.TransactionType = "Withdrawal";

            // Act
            var updatedTransaction = await _transactionRepo.Update(transaction);

            // Assert
            Assert.IsNotNull(updatedTransaction);
            Assert.AreEqual(transaction.Amount, updatedTransaction.Amount);
            Assert.AreEqual(transaction.TransactionType, updatedTransaction.TransactionType);
        }

        [Test, Order(4)]
        public async Task DeleteTransaction_Success()
        {
            // Arrange
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Amount = 100,
                TransactionType = "Deposit",
                Timestamp = DateTime.Now,
                IsRecurring = false
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            // Act
            var deletedTransaction = await _transactionRepo.Delete(transaction.Id);

            // Assert
            Assert.IsNotNull(deletedTransaction);
            Assert.AreEqual(transaction.Id, deletedTransaction.Id);
            var transactionFromContext = await _context.Transactions.FindAsync(transaction.Id);
            Assert.IsNull(transactionFromContext);
        }

        [Test, Order(5)]
        public async Task GetAllTransactions_Success()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountId = Guid.NewGuid(),
                    ReceiverId = Guid.NewGuid(),
                    Amount = 100,
                    TransactionType = "Deposit",
                    Timestamp = DateTime.Now,
                    IsRecurring = false
                },
                new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountId = Guid.NewGuid(),
                    ReceiverId = Guid.NewGuid(),
                    Amount = 200,
                    TransactionType = "Withdrawal",
                    Timestamp = DateTime.Now,
                    IsRecurring = false
                }
            };
            _context.Transactions.AddRange(transactions);
            _context.SaveChanges();

            // Act
            var retrievedTransactions = await _transactionRepo.GetAll();

            // Assert
            Assert.IsNotNull(retrievedTransactions);
            Assert.AreEqual(5, retrievedTransactions.Count());
        }
    }
}
