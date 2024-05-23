using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Interfaces;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs;
using System.Security.Claims;
using SimpleBankingSystemAPI.Exceptions;

namespace SimpleBankingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserAsync(Guid.Parse(userId));
            return Ok(user);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _userService.UpdateUserProfileAsync(new Guid(userId!), request);
            return Ok(profile);
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            if(request.OldPassword == request.NewPassword)
            {
                throw new SamePasswordException("New password and Old password can't be the same");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _userService.UpdateUserPasswordAsync(new Guid(userId!), request);
            if (Request.Cookies["jwt-token-banking-app"] != null)
            {
                Response.Cookies.Delete("jwt-token-banking-app");
            }
            return Ok(new { Message = "Password Changed successfully." });
        }
    }
}
