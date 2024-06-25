using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models.DTOs.AuthDTOs;
using System.Security.Claims;
using WatchDog;

namespace SimpleBankingSystemAPI.Controllers.v1
{
    /// <summary>
    /// Controller for handling authentication related requests.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration request.</param>
        /// <returns>The result of the registration request.</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var userDto = await _userService.RegisterAsync(request);
                return Ok(new
                {
                    message = "New User Register Request Has Been Created!",
                    user = userDto
                });
            }
            catch (Exception ex)
            {
                if (ex is UserAlreadyExistsException)
                {
                    return Conflict(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="request">The login request.</param>
        /// <returns>The authentication token.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var token = await _userService.LoginAsync(request);
                Response.Cookies.Append("jwt-token-banking-app", token, new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    MaxAge = TimeSpan.FromMinutes(15),
                    Expires = DateTimeOffset.UtcNow.AddMinutes(15)
                });
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                if(ex is InvalidCredentialException || ex is UserNotActivatedException)
                {
                    return Unauthorized(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        /// <returns>A message indicating successful logout.</returns>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Logout()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Request.Cookies["jwt-token-banking-app"] != null)
                {
                    Response.Cookies.Delete("jwt-token-banking-app", new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(-1)
                    });
                }
                WatchLogger.Log($"{userId} Logged Out!");
                return Ok(new { Message = "Logged out successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
