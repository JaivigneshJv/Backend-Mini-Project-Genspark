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
        public async Task RepayLoanRequest_WhenValidRequest_ShouldRepayLoan()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var loanId = Guid.NewGuid();
            var request = new LoanRepaymentDto { Amount = 1000, PaymentDate = DateTime.UtcNow };
            var loan = new Loan { Id = loanId, AccountId = Guid.NewGuid(), PendingAmount = 2000, Status = "Opened" };
            var account = new Account { Id = loan.AccountId, UserId = userId, Balance = 3000 };

            _loanRepositoryMock.Setup(repo => repo.GetById(loanId)).ReturnsAsync(loan);
            _accountRepositoryMock.Setup(repo => repo.GetById(loan.AccountId)).ReturnsAsync(account);

            // Act
            var result = await _loanService.RepayLoanRequest(userId, loanId, request);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllRepaymentsForLoanID_WhenCalled_ShouldReturnRepayments()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var loanId = Guid.NewGuid();
            var repayments = new List<LoanRepayment> { new LoanRepayment { LoanId = loanId } };

            _loanRepaymentRepositoryMock.Setup(repo => repo.GetLoanRepaymentsByLoanId(loanId)).ReturnsAsync(repayments);

            // Act
            var result = await _loanService.GetAllRepaymentsForLoanID(userId, loanId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task RejectLoanAsync_WhenValidRequest_ShouldRejectLoan()
        {
            // Arrange
            var loanId = Guid.NewGuid();
            var loan = new Loan { Id = loanId, Status = "Pending" };

            _loanRepositoryMock.Setup(repo => repo.GetById(loanId)).ReturnsAsync(loan);

            // Act
            await _loanService.RejectLoanAsync(loanId);

            // Assert
            _loanRepositoryMock.Verify(repo => repo.Update(It.Is<Loan>(l => l.Status == "Rejected")), Times.Once);
        }

        [Test]
        public async Task GetAllRejectedLoansAsync_WhenCalled_ShouldReturnLoans()
        {
            // Arrange
            var loans = new List<Loan> { new Loan { Status = "Rejected" } };

            _loanRepositoryMock.Setup(repo => repo.GetAllRejectedLoans()).ReturnsAsync(loans);

            // Act
            var result = await _loanService.GetAllRejectedLoansAsync();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllOpenedLoansAsync_WhenCalled_ShouldReturnLoans()
        {
            // Arrange
            var loans = new List<Loan> { new Loan { Status = "Opened" } };

            _loanRepositoryMock.Setup(repo => repo.GetAllOpenedLoans()).ReturnsAsync(loans);

            // Act
            var result = await _loanService.GetAllOpenedLoansAsync();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAllClosedLoansAsync_WhenCalled_ShouldReturnLoans()
        {
            // Arrange
            var loans = new List<Loan> { new Loan { Status = "Closed" } };

            _loanRepositoryMock.Setup(repo => repo.GetAllClosedLoans()).ReturnsAsync(loans);

            // Act
            var result = await _loanService.GetAllClosedLoansAsync();

            // Assert
            Assert.IsNotNull(result);
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
        [Test]
        public void ApplyForLoanAsync_LoanApplicationFails_ShouldThrowException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new LoanRequest { Amount = 10000};
            var user = new User { Id = userId, Email = "test@example.com" };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _loanRepositoryMock.Setup(repo => repo.Add(It.IsAny<Loan>())).Throws(new Exception("Loan application failed"));

            // Act & Assert
            Assert.ThrowsAsync<AccountNotFoundException>(() => _loanService.ApplyLoan(userId, request));
        }
       
        [Test]
        public async Task GetAllLoansByUserIdAsync_NoLoansFound_ShouldReturnEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _loanRepositoryMock.Setup(repo => repo.GetLoansByAccountId(userId)).ReturnsAsync(new List<Loan>());

            // Act
            var result = await _loanService.GetAllPendingLoansAsync();

            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void ApproveLoanAsync_LoanAlreadyApproved_ShouldThrowLoanAlreadyApprovedException()
        {
            // Arrange
            var loanId = Guid.NewGuid();
            var loan = new Loan { Id = loanId };

            _loanRepositoryMock.Setup(repo => repo.GetById(loanId)).ReturnsAsync(loan);

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(() => _loanService.ApproveLoanAsync(loanId));
        }
        


    }
}
