using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBankingSystemUnitTest.Repository
{
    public class TransactionRepositoryTest
    {
        private BankingContext _context;
        private ITransactionRepository _transactionRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder<BankingContext> optionsBuilder = new DbContextOptionsBuilder<BankingContext>()
                .UseInMemoryDatabase("dummyTransactionRepositoryDB");
            _context = new BankingContext(optionsBuilder.Options);
            _transactionRepo = new TransactionRepository(_context);
        }

      

        [Test, Order(2)]
        public async Task GetTransactionsByAccountIdAsync_NoTransactions()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            // Act
            var result = await _transactionRepo.GetTransactionsByAccountIdAsync(accountId);

            // Assert
            Assert.IsEmpty(result);
        }
    }
}
