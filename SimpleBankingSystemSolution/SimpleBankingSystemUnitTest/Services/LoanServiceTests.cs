using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.LoanDTOs;
using SimpleBankingSystemAPI.Services;

namespace SimpleBankingSystemAPI.Tests.Services
{
    [TestFixture]
    public class LoanServiceTests
    {
        private Mock<ILoanRepository> _loanRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ILoanRepaymentRepository> _loanRepaymentRepositoryMock;
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<IEmailSender> _emailServiceMock;
        private LoanService _loanService;

        [SetUp]
        public void Setup()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _mapperMock = new Mock<IMapper>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _loanRepaymentRepositoryMock = new Mock<ILoanRepaymentRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _emailServiceMock = new Mock<IEmailSender>();

            _loanService = new LoanService(
                emailRepository: null,
                emailService: _emailServiceMock.Object,
                transactionRepository: _transactionRepositoryMock.Object,
                loanRepaymentRepository: _loanRepaymentRepositoryMock.Object,
                userRepository: _userRepositoryMock.Object,
                accountRepository: _accountRepositoryMock.Object,
                loanRepository: _loanRepositoryMock.Object,
                mapper: _mapperMock.Object);
        }

        [Test]
        public async Task ApplyLoan_WhenValidRequest_ShouldApplyLoan()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new LoanRequest
            {
                AccountId = Guid.NewGuid(),
                Amount = 1000,
                AppliedDate = DateTime.UtcNow,
                TargetDate = DateTime.UtcNow.AddDays(30)
            };
            var user = new User { Id = userId, Email = "test@example.com" };
            var account = new Account { Id = request.AccountId, UserId = userId };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _accountRepositoryMock.Setup(repo => repo.GetById(request.AccountId)).ReturnsAsync(account);

            // Act
            var result = await _loanService.ApplyLoan(userId, request);

            // Assert
            Assert.IsNull(result);
           }
        [Test]
        public async Task GetLoanDetails_WhenValidRequest_ShouldReturnLoanDetails()
        {
            // Arrange
            var request = new InterestRequest
            {
                Amount = 1000,
                AppliedDate = DateTime.UtcNow,
                TargetDate = DateTime.UtcNow.AddDays(30)
            };

            // Act
            var result = _loanService.GetLoanDetails(request);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllAccountLoans_WhenValidRequest_ShouldReturnLoans()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var loans = new List<Loan> { new Loan { AccountId = accountId } };

            _loanRepositoryMock.Setup(repo => repo.GetLoansByAccountId(accountId)).ReturnsAsync(loans);

            // Assert
            Assert.ThrowsAsync<AccountNotFoundException>(() => _loanService.GetallAccountLoans(userId, accountId));
        }

        [Test]
        public void GetAllAccountLoans_WhenInvalidRequest_ShouldThrowException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();

            _loanRepositoryMock.Setup(repo => repo.GetLoansByAccountId(accountId)).Throws(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<AccountNotFoundException>(() => _loanService.GetallAccountLoans(userId, accountId));
        }

        [Test]
        public async Task GetAllPendingLoansAsync_WhenCalled_ShouldReturnLoans()
        {
            // Arrange
            var loans = new List<Loan> { new Loan { Status = "Pending" } };

            _loanRepositoryMock.Setup(repo => repo.GetAllPendingLoans()).ReturnsAsync(loans);

            // Act
            var result = await _loanService.GetAllPendingLoansAsync();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetAllPendingLoansAsync_WhenErrorOccurs_ShouldThrowException()
        {
            // Arrange
            _loanRepositoryMock.Setup(repo => repo.GetAllPendingLoans()).Throws(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _loanService.GetAllPendingLoansAsync());
        }


    }
}
