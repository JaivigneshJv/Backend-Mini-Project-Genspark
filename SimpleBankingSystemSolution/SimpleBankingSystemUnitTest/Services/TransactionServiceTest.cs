using AutoMapper;
using Moq;
using NUnit.Framework;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs;
using SimpleBankingSystemAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBankingSystemAPI.Tests
{
    [TestFixture]
    public class TransactionServiceTests
    {
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IEmailVerificationRepository> _emailRepositoryMock;
        private Mock<IEmailSender> _emailServiceMock;
        private Mock<ITransactionVerificationRepository> _transactionVerificationRepositoryMock;
        private Mock<IPendingAccountTransactionRepository> _pendingAccountTransactionRepositoryMock;
        private TransactionService _transactionService;

        [SetUp]
        public void SetUp()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _emailRepositoryMock = new Mock<IEmailVerificationRepository>();
            _emailServiceMock = new Mock<IEmailSender>();
            _transactionVerificationRepositoryMock = new Mock<ITransactionVerificationRepository>();
            _pendingAccountTransactionRepositoryMock = new Mock<IPendingAccountTransactionRepository>();

            _transactionService = new TransactionService(
                _transactionVerificationRepositoryMock.Object,
                _userRepositoryMock.Object,
                _accountRepositoryMock.Object,
                _emailRepositoryMock.Object,
                _emailServiceMock.Object,
                _mapperMock.Object,
                _transactionRepositoryMock.Object,
                _pendingAccountTransactionRepositoryMock.Object);
        }

        [Test]
        public async Task DepositAsync_ValidRequest_ShouldDepositAmount()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var request = new DepositRequest { Amount = 1000 };
            var user = new User { Id = userId, Email = "test@example.com" };
            var account = new Account { Id = accountId, UserId = userId, Balance = 500, isActive = true };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _accountRepositoryMock.Setup(repo => repo.GetById(accountId)).ReturnsAsync(account);

            // Act
            await _transactionService.DepositAsync(userId, accountId, request);

            // Assert
            _accountRepositoryMock.Verify(repo => repo.Update(It.Is<Account>(a => a.Balance == 1500)), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.Add(It.IsAny<Transaction>()), Times.Once);
            _emailServiceMock.Verify(service => service.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void DepositAsync_InvalidAccount_ShouldThrowAccountNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var request = new DepositRequest { Amount = 1000 };

            _accountRepositoryMock.Setup(repo => repo.GetById(accountId)).ReturnsAsync((Account)null);

            // Act & Assert
            Assert.ThrowsAsync<AccountNotFoundException>(() => _transactionService.DepositAsync(userId, accountId, request));
        }

        [Test]
        public async Task WithdrawAsync_ValidRequest_ShouldWithdrawAmount()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var request = new DepositRequest { Amount = 500 };
            var user = new User { Id = userId, Email = "test@example.com" };
            var account = new Account { Id = accountId, UserId = userId, Balance = 1000, isActive = true };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _accountRepositoryMock.Setup(repo => repo.GetById(accountId)).ReturnsAsync(account);

            // Act
            await _transactionService.WithdrawAsync(userId, accountId, request);

            // Assert
            _accountRepositoryMock.Verify(repo => repo.Update(It.Is<Account>(a => a.Balance == 500)), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.Add(It.IsAny<Transaction>()), Times.Once);
            _emailServiceMock.Verify(service => service.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void WithdrawAsync_InsufficientFunds_ShouldThrowInsufficientFundsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var request = new DepositRequest { Amount = 1500 };
            var account = new Account { Id = accountId, UserId = userId, Balance = 1000, isActive = true };

            _accountRepositoryMock.Setup(repo => repo.GetById(accountId)).ReturnsAsync(account);

            // Act & Assert
            Assert.ThrowsAsync<InsufficientFundsException>(() => _transactionService.WithdrawAsync(userId, accountId, request));
        }

        [Test]
        public async Task TransferAsync_ValidRequest_ShouldInitiateTransfer()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var receiverId = Guid.NewGuid();
            var request = new BankTransferRequest { Amount = 500, TransactionType = "IMPS" };
            var user = new User { Id = userId, Email = "test@example.com" };
            var account = new Account { Id = accountId, UserId = userId, Balance = 1000, isActive = true };
            var receiverAccount = new Account { Id = receiverId, UserId = Guid.NewGuid(), Balance = 500, isActive = true };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _accountRepositoryMock.Setup(repo => repo.GetById(accountId)).ReturnsAsync(account);
            _accountRepositoryMock.Setup(repo => repo.GetById(receiverId)).ReturnsAsync(receiverAccount);
            _transactionVerificationRepositoryMock.Setup(repo => repo.GetVerificationByAccountIdAsync(accountId)).ReturnsAsync((TransactionVerification)null);

            // Act
            await _transactionService.TransferAsync(userId, accountId, receiverId, request);

            // Assert
            _transactionVerificationRepositoryMock.Verify(repo => repo.Add(It.IsAny<TransactionVerification>()), Times.Once);
            _emailServiceMock.Verify(service => service.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void TransferAsync_InvalidTransactionType_ShouldThrowInvalidTransactionException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var receiverId = Guid.NewGuid();
            var request = new BankTransferRequest { Amount = 500, TransactionType = "INVALID" };

            // Act & Assert
            Assert.ThrowsAsync<InvalidTransactionException>(() => _transactionService.TransferAsync(userId, accountId, receiverId, request));
        }


        [Test]
        public void TransferVerificationAsync_InvalidCode_ShouldThrowInvalidVerificationCodeException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var verificationCode = "123456";
            var user = new User { Id = userId, Email = "test@example.com" };
            var account = new Account { Id = accountId, UserId = userId, Balance = 1000, isActive = true };
            var verification = new TransactionVerification
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                ReceiverId = Guid.NewGuid(),
                Amount = 500,
                TransactionType = "IMPS",
                VerificationCode = "wrongcode",
                Timestamp = DateTime.UtcNow
            };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _accountRepositoryMock.Setup(repo => repo.GetById(accountId)).ReturnsAsync(account);
            _transactionVerificationRepositoryMock.Setup(repo => repo.GetVerificationByAccountIdAsync(accountId)).ReturnsAsync(verification);

            // Act & Assert
            Assert.ThrowsAsync<InvalidVerificationCodeException>(() => _transactionService.TransferVerificationAsync(userId, accountId, verificationCode));
        }
    }
}
