using AutoMapper;
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

namespace SimpleBankingSystemAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailService;
        private readonly IEmailVerificationRepository _emailRepository;

        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, IEmailSender emailService,IEmailVerificationRepository emailRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
            _emailRepository = emailRepository;

        }

        public async Task<UserProfileDto> RegisterAsync(RegisterRequest request)
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
            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Username!);

            if (user == null || !VerifyPasswordHash(request.Password!, user.PasswordHash!, user.PasswordSalt!))
                throw new InvalidCredentialException("Invalid credentials");

            if (!user.IsActive)
                throw new UserNotActivatedException("User not activated");

            return GenerateJwtToken(user);
        }

        public async Task<UserProfileDto> GetUserAsync(Guid userId)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
                throw new UserNotFoundException("User not found");
            return _mapper.Map<UserProfileDto>(user);

        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request)
        {
            var user = await  _userRepository.GetById(userId);

            if (user == null)
                throw new UserNotFoundException("User not found");

            _mapper.Map(request, user);
            user.UpdatedDate = DateTime.UtcNow;
            await _userRepository.Update(user);
            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task UpdateUserPasswordAsync(Guid userId, UpdatePasswordRequest request)
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
        }

        public async Task RequestEmailUpdateAsync(Guid userId, string newEmail)
        {
            var user = await  _userRepository.GetById(userId);

            if (await _userRepository.GetUserByEmailAsync(newEmail) != null)
                throw new EmailAlreadyExistsException("Email already taken");

            if (await _emailRepository.EmailVerificationExists(userId))
                throw new EmailVerificationAlreadyExistsException("Email verification already submiited");

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
        }

        public async Task VerifyEmailUpdateAsync(Guid userId, string verificationCode)
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
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<IEnumerable<UserProfileDto>>(users);
        }
        public async Task<IEnumerable<UserProfileDto>> GetAllInActiveUsersAsync()
        {
            var users = await _userRepository.GetAllInActiveUsers();
            return _mapper.Map<IEnumerable<UserProfileDto>>(users);
        }
        public async Task <IEnumerable<UserProfileDto>> GetAllActiveUsersAsync()
        {
            var users = await _userRepository.GetAllActiveUsers();
            return _mapper.Map<IEnumerable<UserProfileDto>>(users);
        }
        public async Task ActivateUser(Guid userId)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
                throw new UserNotFoundException("User not found");
            user.IsActive = true;
            await _userRepository.Update(user);
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