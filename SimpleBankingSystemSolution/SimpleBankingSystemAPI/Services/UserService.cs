﻿using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs;
using SimpleBankingSystemAPI.Models.DTOs.AuthDTOs;
using SimpleBankingSystemAPI.Repositories;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using WatchDog;

namespace SimpleBankingSystemAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailService;
        private readonly IEmailVerificationRepository _emailRepository;

        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, IEmailSender emailService, IEmailVerificationRepository emailRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
            _emailRepository = emailRepository;
        }

        public async Task<UserProfileDto> RegisterAsync(RegisterRequest request)
        {
            try
            {
                if (await _userRepository.UserExistsAsync(request.Username!))
                    throw new UserAlreadyExistsException("Username already taken");

                CreatePasswordHash(request.Password!, out byte[] passwordHash, out byte[] passwordSalt);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = request.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Contact = request.Contact,
                    DateOfBirth = request.DateOfBirth,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    Role = "User",
                    IsActive = false
                };

                await _userRepository.Add(user);
                WatchLogger.Log($"User registered successfully: {user.Username}");
                return _mapper.Map<UserProfileDto>(user);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error during user registration: {ex}");
                throw;
            }
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(request.Username!);

                if (user == null || !VerifyPasswordHash(request.Password!, user.PasswordHash!, user.PasswordSalt!))
                    throw new InvalidCredentialException("Invalid credentials");

                if (!user.IsActive)
                    throw new UserNotActivatedException("User not activated");

                var token = GenerateJwtToken(user);
                WatchLogger.Log($"User logged in successfully: {user.Username}");
                return token;
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error during user login: {ex}");
                throw;
            }
        }

        public async Task<UserProfileDto> GetUserAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetById(userId);
                if (user == null)
                    throw new UserNotFoundException("User not found");

                WatchLogger.Log($"Fetched user profile: {userId}");
                return _mapper.Map<UserProfileDto>(user);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching user profile: {ex}");
                throw;
            }
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request)
        {
            try
            {
                var user = await _userRepository.GetById(userId);

                if (user == null)
                    throw new UserNotFoundException("User not found");

                _mapper.Map(request, user);
                user.UpdatedDate = DateTime.UtcNow;
                await _userRepository.Update(user);
                WatchLogger.Log($"User profile updated: {userId}");
                return _mapper.Map<UserProfileDto>(user);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error updating user profile: {ex}");
                throw;
            }
        }

        public async Task UpdateUserPasswordAsync(Guid userId, UpdatePasswordRequest request)
        {
            try
            {
                var user = await _userRepository.GetById(userId);

                if (user == null)
                    throw new UserNotFoundException("User not found");

                if (!VerifyPasswordHash(request.OldPassword!, user.PasswordHash!, user.PasswordSalt!))
                    throw new InvalidCredentialException("Old password is incorrect");

                CreatePasswordHash(request.NewPassword!, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.UpdatedDate = DateTime.UtcNow;

                await _userRepository.Update(user);
                WatchLogger.Log($"User password updated: {userId}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error updating user password: {ex}");
                throw;
            }
        }

        public async Task RequestEmailUpdateAsync(Guid userId, string newEmail)
        {
            try
            {
                var user = await _userRepository.GetById(userId);

                if (await _userRepository.GetUserByEmailAsync(newEmail) != null)
                    throw new EmailAlreadyExistsException("Email already taken");

                if (await _emailRepository.EmailVerificationExists(userId))
                    throw new EmailVerificationAlreadyExistsException("Email verification already submitted");

                if (user == null)
                    throw new UserNotFoundException("User not found");

                var verificationCode = new Random().Next(100000, 999999).ToString();
                var emailVerification = new EmailVerification
                {
                    UserId = userId,
                    NewEmail = newEmail,
                    VerificationCode = verificationCode,
                    RequestDate = DateTime.UtcNow
                };

                await _emailService.SendEmailAsync(newEmail, "Verify your new email [Change Request]", $"Your verification code is: {verificationCode}");
                await _emailRepository.Add(emailVerification);
                WatchLogger.Log($"Email update requested for user: {userId}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error requesting email update: {ex}");
                throw;
            }
        }

        public async Task VerifyEmailUpdateAsync(Guid userId, string verificationCode)
        {
            try
            {
                var emailVerification = await _emailRepository.GetUserByUserIdAsync(userId);

                if (emailVerification.VerificationCode != verificationCode)
                    throw new InvalidEmailVerificationCode("Invalid verification code");

                if (emailVerification == null)
                    throw new EmailVerificationNotFoundException("Email verification not found");

                var user = await _userRepository.GetById(userId);

                if (user == null)
                    throw new UserNotFoundException("User not found");

                user.Email = emailVerification.NewEmail;
                user.UpdatedDate = DateTime.UtcNow;

                await _emailRepository.Delete(userId);
                WatchLogger.Log($"Email updated for user: {userId}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error verifying email update: {ex}");
                throw;
            }
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllAsync()
        {
            try
            {
                var users = await _userRepository.GetAll();
                WatchLogger.Log("Fetched all users");
                return _mapper.Map<IEnumerable<UserProfileDto>>(users);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching all users: {ex}");
                throw;
            }
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllInActiveUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllInActiveUsers();
                WatchLogger.Log("Fetched all inactive users");
                return _mapper.Map<IEnumerable<UserProfileDto>>(users);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching inactive users: {ex}");
                throw;
            }
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllActiveUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllActiveUsers();
                WatchLogger.Log("Fetched all active users");
                return _mapper.Map<IEnumerable<UserProfileDto>>(users);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching active users: {ex}");
                throw;
            }
        }

        public async Task ActivateUser(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetById(userId);
                if (user == null)
                    throw new UserNotFoundException("User not found");

                user.IsActive = true;
                await _userRepository.Update(user);
                WatchLogger.Log($"User activated: {userId}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error activating user: {ex}");
                throw;
            }
        }

        #region PasswordHashing
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username!),
                new Claim(ClaimTypes.Role, user.Role!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey:JWT"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
