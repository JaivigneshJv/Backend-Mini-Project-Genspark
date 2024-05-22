using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces;
using SimpleBankingSystemAPI.Models.DTOs;
using SimpleBankingSystemAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Exceptions;

namespace SimpleBankingSystemAPI.Services
{
    public class UserService : IUserService
    {
        private readonly BankingContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(BankingContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<UserDto> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users!.AnyAsync(u => u.Username == request.Username))
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

            _context.Users!.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users!.SingleOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !VerifyPasswordHash(request.Password!, user.PasswordHash!, user.PasswordSalt!))
                throw new InvalidCredentialException("Invalid credentials");

            if (!user.IsActive)
                throw new UserNotActivatedException("User not activated");

            return GenerateJwtToken(user);
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