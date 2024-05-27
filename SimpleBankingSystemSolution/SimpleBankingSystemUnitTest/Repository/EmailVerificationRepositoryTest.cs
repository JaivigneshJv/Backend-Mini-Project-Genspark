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
    public class EmailVerificationRepositoryTest
    {
        BankingContext context;
        IEmailVerificationRepository emailVerificationRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder<BankingContext> optionsBuilder = new DbContextOptionsBuilder<BankingContext>()
                                                            .UseInMemoryDatabase("dummyEmailVerificationRepositoryDB");
            context = new BankingContext(optionsBuilder.Options);
            emailVerificationRepo = new EmailVerificationRepository(context);
        }

        [Test, Order(1)]
        public async Task EmailVerificationExists_Success()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            context.EmailVerifications.Add(new EmailVerification
            {
                UserId = userId,
                NewEmail = "test@test.com",
                VerificationCode = "123456",
                RequestDate = DateTime.Now
            });
            context.SaveChanges();

            // Act
            var result = await emailVerificationRepo.EmailVerificationExists(userId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test, Order(2)]
        public async Task EmailVerificationExists_Failure()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            // Act
            var result = await emailVerificationRepo.EmailVerificationExists(userId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test, Order(3)]
        public async Task GetUserByUserIdAsync_Success()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            context.EmailVerifications.Add(new EmailVerification
            {
                UserId = userId,
                NewEmail = "test@test.com",
                VerificationCode = "123456",
                RequestDate = DateTime.Now
            });
            context.SaveChanges();

            // Act
            var result = await emailVerificationRepo.GetUserByUserIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test, Order(4)]
        public async Task GetUserByUserIdAsync_Failure()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            // Act
            var result = await emailVerificationRepo.GetUserByUserIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }
    }
}
