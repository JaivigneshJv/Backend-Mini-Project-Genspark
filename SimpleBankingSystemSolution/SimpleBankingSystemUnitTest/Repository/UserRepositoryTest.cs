using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBankingSystemUnitTest.Repository
{
    public class UserRepositoryTest
    {
        BankingContext context;
        IUserRepository userRepo;

        [SetUp]
        public void SetUp()
        {

            DbContextOptionsBuilder<BankingContext> optionsBuilder = new DbContextOptionsBuilder<BankingContext>()
                                                            .UseInMemoryDatabase("dummyUserRepositoryDB");
            context = new BankingContext(optionsBuilder.Options);
            userRepo = new UserRepository(context);
        }
        [Test, Order(1)]
        public async Task GetUserByUsernameAsync_Success()
        {
            Assert.IsNull(await userRepo.GetUserByUsernameAsync("jv"));
        }
        [Test, Order(2)]
        public async Task GetUserByUsernameAsync_Failure()
        {
            // Arrange
            string username = "nonexistent";

            // Act
            var result = await userRepo.GetUserByUsernameAsync(username);

            // Assert
            Assert.IsNull(result);
        }

        [Test, Order(3)]
        public async Task GetUserByUsernameAsync_Exception()
        {
            // Arrange
            string username = "exception";

            context.Users.Add(new User
            {
                Username = username,
                Contact = "1234567890",
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = new byte[0],
                PasswordSalt = new byte[0],
                Role = "TestRole"
            });
            context.SaveChanges();

            // Act & Assert
            var user = await userRepo.GetUserByUsernameAsync(username);
            Assert.That(user.Contact, Is.EqualTo("1234567890"));
        }


        [Test, Order(4)]
        public async Task UserExistsAsync_Success()
        {
            // Arrange
            string username = "jv";
            context.Users.Add(new User
            {
                Username = username,
                Contact = "1234567890",
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = new byte[0],
                PasswordSalt = new byte[0],
                Role = "TestRole"
            });
            context.SaveChanges();

            // Act
            var result = await userRepo.UserExistsAsync(username);

            // Assert
            Assert.IsTrue(result);
        }


        [Test, Order(5)]
        public async Task UserExistsAsync_Failure()
        {
            // Arrange
            string username = "nonexistent";

            // Act
            var result = await userRepo.UserExistsAsync(username);

            // Assert
            Assert.IsFalse(result);
        }

        [Test, Order(6)]
        public void UserExistsAsync_Exception()
        {
            // Arrange
            string username = "exception";
            context.Users.Add(new User
            {
                Username = username,
                Contact = "1234567890",
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = new byte[0],
                PasswordSalt = new byte[0],
                Role = "TestRole"
            });
            context.SaveChanges();

            // Act & Assert
            Assert.That(async () => await userRepo.UserExistsAsync(username),Is.EqualTo(true));
        }

        [Test, Order(8)]
        public async Task GetUserByEmailAsync_Failure()
        {
            // Arrange
            string email = "nonexistent@test.com";

            // Act
            var result = await userRepo.GetUserByEmailAsync(email);

            // Assert
            Assert.IsNull(result);
        }

        [Test, Order(9)]
        public async Task GetUserByEmailAsync_Exception()
        {
            // Arrange
            string email = "exception@test.com";
            context.Users.Add(new User
            {
                Email = email,
                Username = "test",
                Contact = "1234567890",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = new byte[0],
                PasswordSalt = new byte[0],
                Role = "TestRole"
            });
            context.SaveChanges();
            // Act & Assert
            var users = await userRepo.GetUserByEmailAsync(email);
            Assert.IsNotNull(users);

        }

        [Test, Order(10)]
        public async Task GetAllInActiveUsers_Success()
        {
            // Arrange
            context.Users.Add(new User
            {
                Email = "test@test.com",
                Username = "test",
                Contact = "1234567890",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = new byte[0],
                PasswordSalt = new byte[0],
                Role = "TestRole",
                IsActive = false
            });
            context.SaveChanges();

            // Act
            var result = await userRepo.GetAllInActiveUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());
        }

        [Test, Order(11)]
        public async Task GetAllInActiveUsers_Exception()
        {
            // Arrange
            context.Users.Add(new User
            {
                Email = "exception@test.com",
                Username = "test",
                Contact = "1234567890",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = new byte[0],
                PasswordSalt = new byte[0],
                Role = "TestRole",
                IsActive = false
            });
            context.SaveChanges();

            // Act & Assert
            var users = await userRepo.GetAllInActiveUsers();
            Assert.IsNotNull(users);
        }

        [Test, Order(12)]
        public async Task GetAllActiveUsers_Success()
        {
            // Arrange
            context.Users.Add(new User
            {
                Email = "test@test.com",
                Username = "test",
                Contact = "1234567890",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = new byte[0],
                PasswordSalt = new byte[0],
                Role = "TestRole",
                IsActive = true
            });
            context.SaveChanges();

            // Act
            var result = await userRepo.GetAllActiveUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test, Order(13)]
        public async Task GetAllActiveUsers_Exception()
        {
            // Arrange
            context.Users.Add(new User
            {
                Email = "exception@test.com",
                Username = "test",
                Contact = "1234567890",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = new byte[0],
                PasswordSalt = new byte[0],
                Role = "TestRole",
                IsActive = true
            });
            context.SaveChanges();

            // Act & Assert
            var users = await userRepo.GetAllActiveUsers();
            Assert.IsNotNull(users);
        }



    }
}