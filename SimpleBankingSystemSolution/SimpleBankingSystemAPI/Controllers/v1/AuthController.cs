using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models.DTOs.AuthDTOs;

namespace SimpleBankingSystemAPI.Controllers.v1
{
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

        [HttpPost("register")]
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
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var token = await _userService.LoginAsync(request);
                Response.Cookies.Append("jwt-token-banking-app", token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                    MaxAge = TimeSpan.FromMinutes(15)
                });
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies["jwt-token-banking-app"] != null)
            {
                Response.Cookies.Delete("jwt-token-banking-app");
            }
            return Ok(new { Message = "Logged out successfully." });
        }
    }
}
