using AutoMapper;
using Moq;
using NUnit.Framework;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBankingSystemUnitTest.Services
{
    public class AccountServiceTests
    {
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IPendingAccountClosingRepository> _pendingAccountClosingRepositoryMock;
        private IAccountService _accountService;

        
        [SetUp]
        public void SetUp()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _pendingAccountClosingRepositoryMock = new Mock<IPendingAccountClosingRepository>();
            _accountService = new AccountService(
                _pendingAccountClosingRepositoryMock.Object,
                _userRepositoryMock.Object,
                _accountRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public async Task OpenAccountAsync_WhenUserNotFound_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new OpenAccountRequest { AccountType = "Savings", InitialDeposit = 1000, TransactionPassword = "password" };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await _accountService.OpenAccountAsync(userId, request));
        }


        [Test]
        public async Task GetAccountAsync_WhenAccountNotFound_ShouldThrowAccountNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();

            _accountRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((Account)null);

            // Act & Assert
            Assert.ThrowsAsync<AccountNotFoundException>(async () => await _accountService.GetAccountAsync(userId, accountId));
        }

        [Test]
        public async Task GetAccountAsync_WhenValidRequest_ShouldReturnAccountDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var account = new Account { Id = accountId, UserId = userId };

            _accountRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(account);
            _mapperMock.Setup(x => x.Map<AccountDto>(It.IsAny<Account>())).Returns(new AccountDto());

            // Act
            var result = await _accountService.GetAccountAsync(userId, accountId);

            // Assert
            Assert.IsNotNull(result);
            _accountRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public async Task RequestCloseAccountAsync_WhenAccountNotFound_ShouldThrowAccountNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();

            _accountRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((Account)null);

            // Act & Assert
            Assert.ThrowsAsync<AccountNotFoundException>(async () => await _accountService.RequestCloseAccountAsync(userId, accountId));
        }

      

        [Test]
        public async Task GetAccountsAsync_WhenValidRequest_ShouldReturnAccountsDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accounts = new List<Account> { new Account { Id = Guid.NewGuid(), UserId = userId } };

            _accountRepositoryMock.Setup(x => x.GetAccountsByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(accounts);
            _mapperMock.Setup(x => x.Map<IEnumerable<AccountDto>>(It.IsAny<IEnumerable<Account>>())).Returns(new List<AccountDto>());

            // Act
            var result = await _accountService.GetAccountsAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            _accountRepositoryMock.Verify(x => x.GetAccountsByUserIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public async Task GetAllAccountsAsync_WhenValidRequest_ShouldReturnAllAccountsDto()
        {
            // Arrange
            var accounts = new List<Account> { new Account { Id = Guid.NewGuid() } };

            _accountRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(accounts);
            _mapperMock.Setup(x => x.Map<IEnumerable<AccountDto>>(It.IsAny<IEnumerable<Account>>())).Returns(new List<AccountDto>());

            // Act
            var result = await _accountService.GetAllAccountsAsync();

            // Assert
            Assert.IsNotNull(result);
            _accountRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

     

        [Test]
        public async Task UpdateAccountAsync_WhenAccountNotFound_ShouldThrowAccountNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var request = new UpdateAccountRequest { TransactionPassword = "newPassword" };

            _accountRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((Account)null);

            // Act & Assert
            Assert.ThrowsAsync<AccountNotFoundException>(async () => await _accountService.UpdateAccountAsync(userId, accountId, request));
        }

      

        [Test]
        public async Task CloseAccountAsync_WhenAccountHasBalance_ShouldThrowAccountHasBalanceException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var request = new AccountClosingDto { AccountId = accountId, AccountType = "Savings" };
            var account = new Account { Id = accountId, UserId = userId, Balance = 1000 };

            _accountRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(account);

            // Act & Assert
            Assert.ThrowsAsync<AccountHasBalanceException>(async () => await _accountService.CloseAccountAsync(userId, accountId, request));
        }

        [Test]
        public void OpenAccountAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new OpenAccountRequest { AccountType = "Savings", InitialDeposit = 1000, TransactionPassword = "password" };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(async () => await _accountService.OpenAccountAsync(userId, request));
        }

        [Test]
        public async Task GetAccountAsync_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var account = new Account { Id = accountId, UserId = userId };

            _accountRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(account);
            _mapperMock.Setup(x => x.Map<AccountDto>(It.IsAny<Account>())).Returns(new AccountDto());

            // Act
            var result = await _accountService.GetAccountAsync(userId, accountId);

            // Assert
            Assert.IsNotNull(result);
            _accountRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void GetAccountAsync_AccountNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();

            _accountRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((Account)null);

            // Act & Assert
            Assert.ThrowsAsync<AccountNotFoundException>(async () => await _accountService.GetAccountAsync(userId, accountId));
        }

        [Test]
        public async Task GetAccountsAsync_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accounts = new List<Account> { new Account { Id = Guid.NewGuid(), UserId = userId } };

            _accountRepositoryMock.Setup(x => x.GetAccountsByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(accounts);
            _mapperMock.Setup(x => x.Map<IEnumerable<AccountDto>>(It.IsAny<IEnumerable<Account>>())).Returns(new List<AccountDto>());

            // Act
            var result = await _accountService.GetAccountsAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            _accountRepositoryMock.Verify(x => x.GetAccountsByUserIdAsync(It.IsAny<Guid>()), Times.Once);
        }
       
    }
}
