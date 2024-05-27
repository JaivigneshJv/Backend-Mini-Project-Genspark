using NUnit.Framework;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs;
using SimpleBankingSystemAPI.Models.DTOs.AuthDTOs;
using SimpleBankingSystemAPI.Services;
using SimpleBankingSystemAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBankingSystemAPI.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IEmailSender> _emailServiceMock;
        private Mock<IEmailVerificationRepository> _emailRepositoryMock;
        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _configurationMock = new Mock<IConfiguration>();
            _emailServiceMock = new Mock<IEmailSender>();
            _emailRepositoryMock = new Mock<IEmailVerificationRepository>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _configurationMock.Object,
                _emailServiceMock.Object,
                _emailRepositoryMock.Object);
        }

        [Test]
        public async Task RegisterAsync_ValidRequest_ShouldRegisterUser()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Username = "testuser",
                Password = "Password123!",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Contact = "1234567890",
                DateOfBirth = DateTime.UtcNow.AddYears(-25)
            };

            _userRepositoryMock.Setup(repo => repo.UserExistsAsync(request.Username)).ReturnsAsync(false);
            _mapperMock.Setup(m => m.Map<UserProfileDto>(It.IsAny<User>())).Returns(new UserProfileDto { Username = request.Username });

            // Act
            var result = await _userService.RegisterAsync(request);

            // Assert
            Assert.AreEqual(request.Username, result.Username);
            _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void RegisterAsync_UserAlreadyExists_ShouldThrowUserAlreadyExistsException()
        {
            // Arrange
            var request = new RegisterRequest { Username = "existinguser" };

            _userRepositoryMock.Setup(repo => repo.UserExistsAsync(request.Username)).ReturnsAsync(true);

            // Act & Assert
            Assert.ThrowsAsync<UserAlreadyExistsException>(() => _userService.RegisterAsync(request));
        }

    

        [Test]
        public void LoginAsync_InvalidCredentials_ShouldThrowInvalidCredentialException()
        {
            // Arrange
            var request = new LoginRequest { Username = "testuser", Password = "WrongPassword!" };
            var user = new User
            {
                Username = request.Username,
                PasswordHash = new byte[64],
                PasswordSalt = new byte[128],
                IsActive = true
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(request.Username)).ReturnsAsync(user);

            // Act & Assert
            Assert.ThrowsAsync<InvalidCredentialException>(() => _userService.LoginAsync(request));
        }

        [Test]
        public async Task GetUserAsync_ValidUserId_ShouldReturnUserProfile()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Username = "testuser" };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserProfileDto>(user)).Returns(new UserProfileDto { Username = user.Username });

            // Act
            var result = await _userService.GetUserAsync(userId);

            // Assert
            Assert.AreEqual(user.Username, result.Username);
        }

        [Test]
        public void GetUserAsync_InvalidUserId_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserAsync(userId));
        }

        [Test]
        public void UpdateUserProfileAsync_InvalidUserId_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdateUserProfileRequest { FirstName = "Updated" };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(() => _userService.UpdateUserProfileAsync(userId, request));
        }

       

        [Test]
        public void UpdateUserPasswordAsync_InvalidOldPassword_ShouldThrowInvalidCredentialException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdatePasswordRequest { OldPassword = "WrongOldPassword!", NewPassword = "NewPassword123!" };
            var user = new User { Id = userId, Username = "testuser", PasswordHash = new byte[64], PasswordSalt = new byte[128] };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);

            // Act & Assert
            Assert.ThrowsAsync<InvalidCredentialException>(() => _userService.UpdateUserPasswordAsync(userId, request));
        }

        [Test]
        public async Task RequestEmailUpdateAsync_ValidRequest_ShouldSendEmailAndUpdateRepository()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newEmail = "newemail@example.com";
            var user = new User { Id = userId, Email = "oldemail@example.com" };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(newEmail)).ReturnsAsync((User)null);
            _emailRepositoryMock.Setup(repo => repo.EmailVerificationExists(userId)).ReturnsAsync(false);

            // Act
            await _userService.RequestEmailUpdateAsync(userId, newEmail);

            // Assert
            _emailServiceMock.Verify(service => service.SendEmailAsync(newEmail, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _emailRepositoryMock.Verify(repo => repo.Add(It.IsAny<EmailVerification>()), Times.Once);
        }

        [Test]
        public void RequestEmailUpdateAsync_EmailAlreadyExists_ShouldThrowEmailAlreadyExistsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newEmail = "newemail@example.com";
            var user = new User { Id = userId, Email = "oldemail@example.com" };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(newEmail)).ReturnsAsync(new User());

            // Act & Assert
            Assert.ThrowsAsync<EmailAlreadyExistsException>(() => _userService.RequestEmailUpdateAsync(userId, newEmail));
        }

        [Test]
        public async Task VerifyEmailUpdateAsync_ValidVerificationCode_ShouldUpdateEmail()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var verificationCode = "123456";
            var emailVerification = new EmailVerification { UserId = userId, VerificationCode = verificationCode, NewEmail = "newemail@example.com" };
            var user = new User { Id = userId, Email = "oldemail@example.com" };

            _emailRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(userId)).ReturnsAsync(emailVerification);
            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);

            // Act
            await _userService.VerifyEmailUpdateAsync(userId, verificationCode);

            // Assert
            Assert.AreEqual(emailVerification.NewEmail, user.Email);
            _emailRepositoryMock.Verify(repo => repo.Delete(userId), Times.Once);
        }

        [Test]
        public void VerifyEmailUpdateAsync_InvalidVerificationCode_ShouldThrowInvalidEmailVerificationCode()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var verificationCode = "654321";
            var emailVerification = new EmailVerification { UserId = userId, VerificationCode = "123456", NewEmail = "newemail@example.com" };

            _emailRepositoryMock.Setup(repo => repo.GetUserByUserIdAsync(userId)).ReturnsAsync(emailVerification);

            // Act & Assert
            Assert.ThrowsAsync<InvalidEmailVerificationCode>(() => _userService.VerifyEmailUpdateAsync(userId, verificationCode));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Username = "user1" },
                new User { Username = "user2" }
            };

            _userRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserProfileDto>>(users)).Returns(new List<UserProfileDto>
            {
                new UserProfileDto { Username = "user1" },
                new UserProfileDto { Username = "user2" }
            });

            // Act
            var result = await _userService.GetAllAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllInActiveUsersAsync_ShouldReturnAllInactiveUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Username = "inactive1", IsActive = false },
                new User { Username = "inactive2", IsActive = false }
            };

            _userRepositoryMock.Setup(repo => repo.GetAllInActiveUsers()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserProfileDto>>(users)).Returns(new List<UserProfileDto>
            {
                new UserProfileDto { Username = "inactive1" },
                new UserProfileDto { Username = "inactive2" }
            });

            // Act
            var result = await _userService.GetAllInActiveUsersAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllActiveUsersAsync_ShouldReturnAllActiveUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Username = "active1", IsActive = true },
                new User { Username = "active2", IsActive = true }
            };

            _userRepositoryMock.Setup(repo => repo.GetAllActiveUsers()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserProfileDto>>(users)).Returns(new List<UserProfileDto>
            {
                new UserProfileDto { Username = "active1" },
                new UserProfileDto { Username = "active2" }
            });

            // Act
            var result = await _userService.GetAllActiveUsersAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task ActivateUser_ValidUserId_ShouldActivateUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, IsActive = false };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);

            // Act
            await _userService.ActivateUser(userId);

            // Assert
            Assert.IsTrue(user.IsActive);
            _userRepositoryMock.Verify(repo => repo.Update(user), Times.Once);
        }

        [Test]
        public void ActivateUser_InvalidUserId_ShouldThrowUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(() => _userService.ActivateUser(userId));
        }
    }
}
