using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs.EmailDTOs;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs;
using System.Security.Claims;
using SimpleBankingSystemAPI.Interfaces.Services;

namespace SimpleBankingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileUpdateController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;

        public UserProfileUpdateController(IUserService userService, IEmailSender emailSender)
        {
            _userService = userService;
            _emailSender = emailSender;
        }

        [Authorize]
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            if (request.OldPassword == request.NewPassword)
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

        [Authorize]
        [HttpPut("email/request-update")]
        public async Task<IActionResult> RequestEmailUpdate([FromBody] RequestEmailUpdate request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _userService.RequestEmailUpdateAsync(new Guid(userId!), request.NewEmail!);
            return Ok(new { Message = "Email update requested. Please check your new email for the verification code." });
        }

        [Authorize]
        [HttpPut("email/verify-update")]
        public async Task<IActionResult> VerifyEmailUpdate([FromBody] VerifyEmailRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _userService.VerifyEmailUpdateAsync(new Guid(userId!), request.VerificationCode!);
            return Ok(new { Message = "Email updated successfully." });
        }
    }

}
