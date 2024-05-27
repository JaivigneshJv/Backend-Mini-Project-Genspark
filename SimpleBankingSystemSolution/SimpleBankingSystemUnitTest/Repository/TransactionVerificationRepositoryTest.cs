using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Repositories;
using System;
using System.Threading.Tasks;

namespace SimpleBankingSystemUnitTest.Repository
{
    public class TransactionVerificationRepositoryTest
    {
        private BankingContext _context;
        private ITransactionVerificationRepository _transactionVerificationRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder<BankingContext> optionsBuilder = new DbContextOptionsBuilder<BankingContext>()
                .UseInMemoryDatabase("dummyTransactionVerificationRepositoryDB");
            _context = new BankingContext(optionsBuilder.Options);
            _transactionVerificationRepo = new TransactionVerificationRepository(_context);
        }

       
        [Test, Order(2)]
        public async Task GetVerificationByAccountIdAsync_NotFound()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            // Act
            var result = await _transactionVerificationRepo.GetVerificationByAccountIdAsync(accountId);

            // Assert
            Assert.IsNull(result);
        }
    }
}
