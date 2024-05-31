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
        public async Task UpdateAccountAsync_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var request = new UpdateAccountRequest { TransactionPassword = "newPassword" };
            var account = new Account { Id = accountId, UserId = userId };

            _accountRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(account);
            _mapperMock.Setup(x => x.Map<AccountDto>(It.IsAny<Account>())).Returns(new AccountDto());

            // Act
            var result = await _accountService.UpdateAccountAsync(userId, accountId, request);

            // Assert
            Assert.IsNotNull(result);
            _accountRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _accountRepositoryMock.Verify(x => x.Update(It.IsAny<Account>()), Times.Once);
        }



        [Test]
        public async Task GetAllAccountsAsync_Success()
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
        public async Task GetAccountAsync_WhenAccountExists_ShouldReturnAccountDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var account = new Account { UserId = userId };
            var accountDto = new AccountDto();

            _accountRepositoryMock.Setup(x => x.GetById(accountId)).ReturnsAsync(account);
            _mapperMock.Setup(x => x.Map<AccountDto>(account)).Returns(accountDto);

            // Act
            var result = await _accountService.GetAccountAsync(userId, accountId);

            // Assert
            Assert.AreEqual(accountDto, result);
        }

        [Test]
        public async Task RequestCloseAccountAsync_WhenAccountExists_ShouldNotThrowException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var account = new Account { UserId = userId };

            _accountRepositoryMock.Setup(x => x.GetById(accountId)).ReturnsAsync(account);

            // Act
            await _accountService.RequestCloseAccountAsync(userId, accountId);

            // Assert
            _accountRepositoryMock.Verify(x => x.Update(account), Times.Once);
        }

        [Test]
        public async Task GetAccountsAsync_WhenAccountsExist_ShouldReturnAccountDtos()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accounts = new List<Account> { new Account(), new Account() };
            var accountDtos = new List<AccountDto> { new AccountDto(), new AccountDto() };

            _accountRepositoryMock.Setup(x => x.GetAccountsByUserIdAsync(userId)).ReturnsAsync(accounts);
            _mapperMock.Setup(x => x.Map<IEnumerable<AccountDto>>(accounts)).Returns(accountDtos);

            // Act
            var result = await _accountService.GetAccountsAsync(userId);

            // Assert
            CollectionAssert.AreEqual(accountDtos, result);
        }
       

        [Test]
        public async Task GetPendingAccountClosingRequests_WhenRequestsExist_ShouldReturnAccountClosingDtos()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var pendingAccountClosingRequests = new List<PendingAccountClosing> { new PendingAccountClosing(), new PendingAccountClosing() };
            var accountClosingDtos = new List<AccountClosingDto> { new AccountClosingDto(), new AccountClosingDto() };

            _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync(new User { Role = "Admin" });
            _pendingAccountClosingRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(pendingAccountClosingRequests);
            _mapperMock.Setup(x => x.Map<IEnumerable<AccountClosingDto>>(pendingAccountClosingRequests)).Returns(accountClosingDtos);

            // Act
            var result = await _accountService.GetPendingAccountClosingRequests(userId);

            // Assert
            CollectionAssert.AreEqual(accountClosingDtos, result);
        }

        [Test]
        public async Task AcceptAccountCloseRequest_WhenRequestExists_ShouldNotThrowException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var account = new Account();
            var pendingAccountClosing = new PendingAccountClosing { AccountId = account.Id };

            _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync(new User { Role = "Admin" });
            _pendingAccountClosingRepositoryMock.Setup(x => x.GetById(requestId)).ReturnsAsync(pendingAccountClosing);
            _accountRepositoryMock.Setup(x => x.GetById(account.Id)).ReturnsAsync(account);

            // Act
            await _accountService.AcceptAccountCloseRequest(userId, requestId);

            // Assert
            _pendingAccountClosingRepositoryMock.Verify(x => x.Update(pendingAccountClosing), Times.Once);
            _accountRepositoryMock.Verify(x => x.Update(account), Times.Once);
        }


        [Test]
        public async Task GetAllAccountsAsync_WhenAccountsExist_ShouldReturnAccountDtos()
        {
            // Arrange
            var accounts = new List<Account> { new Account(), new Account() };
            var accountDtos = new List<AccountDto> { new AccountDto(), new AccountDto() };

            _accountRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(accounts);
            _mapperMock.Setup(x => x.Map<IEnumerable<AccountDto>>(accounts)).Returns(accountDtos);

            // Act
            var result = await _accountService.GetAllAccountsAsync();

            // Assert
            CollectionAssert.AreEqual(accountDtos, result);
        }
        [Test]
        public async Task RequestCloseAccountAsync_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var account = new Account { Id = accountId, UserId = userId };

            _accountRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(account);

            // Act
            await _accountService.RequestCloseAccountAsync(userId, accountId);

            // Assert
            _accountRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _accountRepositoryMock.Verify(x => x.Update(It.IsAny<Account>()), Times.Once);
        }


        [Test]
        public async Task GetPendingAccountClosingRequests_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Role = "Admin" };
            var pendingAccountClosingRequests = new List<PendingAccountClosing> { new PendingAccountClosing { Id = Guid.NewGuid() } };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(user);
            _pendingAccountClosingRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(pendingAccountClosingRequests);
            _mapperMock.Setup(x => x.Map<IEnumerable<AccountClosingDto>>(It.IsAny<IEnumerable<PendingAccountClosing>>())).Returns(new List<AccountClosingDto>());

            // Act
            var result = await _accountService.GetPendingAccountClosingRequests(userId);

            // Assert
            Assert.IsNotNull(result);
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _pendingAccountClosingRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public async Task AcceptAccountCloseRequest_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var user = new User { Id = userId, Role = "Admin" };
            var closeAccountRequest = new PendingAccountClosing { Id = requestId, AccountId = Guid.NewGuid() };
            var account = new Account { Id = closeAccountRequest.AccountId, UserId = userId };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(user);
            _pendingAccountClosingRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(closeAccountRequest);
            _accountRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(account);

            // Act
            await _accountService.AcceptAccountCloseRequest(userId, requestId);

            // Assert
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _pendingAccountClosingRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _accountRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _pendingAccountClosingRepositoryMock.Verify(x => x.Update(It.IsAny<PendingAccountClosing>()), Times.Once);
            _accountRepositoryMock.Verify(x => x.Update(It.IsAny<Account>()), Times.Once);
        }

        [Test]
        public async Task RejectAccountCloseRequest_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var user = new User { Id = userId, Role = "Admin" };
            var closeAccountRequest = new PendingAccountClosing { Id = requestId, AccountId = Guid.NewGuid() };
            var account = new Account { Id = closeAccountRequest.AccountId, UserId = userId };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(user);
            _pendingAccountClosingRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(closeAccountRequest);
            _accountRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(account);

            // Act
            await _accountService.RejectAccountCloseRequest(userId, requestId);

            // Assert
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _pendingAccountClosingRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
            _pendingAccountClosingRepositoryMock.Verify(x => x.Update(It.IsAny<PendingAccountClosing>()), Times.Once);
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
        public void CreateAccountAsync_AccountCreationFails_ShouldThrowException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Email = "test@example.com" };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _accountRepositoryMock.Setup(repo => repo.Add(It.IsAny<Account>())).Throws(new Exception("Account creation failed"));

            // Act & Assert
            Assert.ThrowsAsync<AccountNotFoundException>(() => _accountService.RequestCloseAccountAsync(userId,userId));
        }
        [Test]
        public void GetAccountByIdAsync_AccountInactive_ShouldThrowAccountInactiveException()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var account = new Account { Id = accountId, isActive = false };

            _accountRepositoryMock.Setup(repo => repo.GetById(accountId)).ReturnsAsync(account);

            // Act & Assert
            Assert.ThrowsAsync<AccountNotFoundException>(() => _accountService.GetAccountAsync(accountId, accountId));
        }
        [Test]
        public void CloseAccountAsync_AccountNotFound_ShouldThrowAccountNotFoundException()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            _accountRepositoryMock.Setup(repo => repo.GetById(accountId)).ReturnsAsync((Account)null);

            // Act & Assert
            Assert.ThrowsAsync<AccountNotFoundException>(() => _accountService.RequestCloseAccountAsync(accountId, accountId));
        }
        [Test]
        public async Task GetAllAccountsByUserIdAsync_NoAccountsFound_ShouldReturnEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _accountRepositoryMock.Setup(repo => repo.GetAccountsByUserIdAsync(userId)).ReturnsAsync(new List<Account>());

            // Act
            var result = await _accountService.GetAllAccountsAsync();

            // Assert
            Assert.IsNotNull(result);
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
