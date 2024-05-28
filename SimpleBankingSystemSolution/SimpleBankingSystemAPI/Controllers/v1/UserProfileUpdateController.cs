using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs.EmailDTOs;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs;
using System.Security.Claims;
using SimpleBankingSystemAPI.Interfaces.Services;

namespace SimpleBankingSystemAPI.Controllers.v1
{
    /// <summary>
    /// Controller for updating user profile information.
    /// </summary>
    [Route("api/v1/[controller]")]
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

        /// <summary>
        /// Updates the user's password.
        /// </summary>
        /// <param name="request">The request containing the old and new passwords.</param>
        /// <returns>An IActionResult indicating the result of the password update.</returns>
        [Authorize]
        [HttpPut("update-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            try
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
            catch (Exception ex)
            {
                if(ex is UserNotFoundException)
                {
                    return NotFound(new { message = ex.Message });
                }
                if(ex is InvalidCredentialException)
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Requests an email update for the user.
        /// </summary>
        /// <param name="request">The request containing the new email.</param>
        /// <returns>An IActionResult indicating the result of the email update request.</returns>
        [Authorize]
        [HttpPut("email/request-update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RequestEmailUpdate([FromBody] RequestEmailUpdate request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _userService.RequestEmailUpdateAsync(new Guid(userId!), request.NewEmail!);
                return Ok(new { Message = "Email update requested. Please check your new email for the verification code." });
            }
            catch (Exception ex)
            {
                if(ex is EmailAlreadyExistsException || ex is EmailVerificationAlreadyExistsException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is UserNotFoundException)
                {
                    return NotFound(new { message = ex.Message });
                }

                return StatusCode(500, new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Verifies the updated email for the user.
        /// </summary>
        /// <param name="request">The request containing the verification code.</param>
        /// <returns>An IActionResult indicating the result of the email verification.</returns>
        [Authorize]
        [HttpPut("email/verify-update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerifyEmailUpdate([FromBody] VerifyEmailRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _userService.VerifyEmailUpdateAsync(new Guid(userId!), request.VerificationCode!);
                return Ok(new { Message = "Email updated successfully." });
            }
            catch (Exception ex)
            {
                if(ex is UserNotFoundException || ex is EmailVerificationNotFoundException)
                {
                    return NotFound(new { message = ex.Message });
                }
                if(ex is InvalidEmailVerificationCode)
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }

}
