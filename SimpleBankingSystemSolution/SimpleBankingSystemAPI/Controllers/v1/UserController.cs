using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs;
using System.Security.Claims;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs.EmailDTOs;
using SimpleBankingSystemAPI.Interfaces.Services;

namespace SimpleBankingSystemAPI.Controllers.v1
{
    /// <summary>
    /// Represents the UserController class.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the UserController class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="emailSender">The email sender.</param>
        public UserController(IUserService userService, IEmailSender emailSender)
        {
            _userService = userService;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <returns>The user profile.</returns>
        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userService.GetUserAsync(Guid.Parse(userId));
                return Ok(user);
            }
            catch (Exception ex)
            {
                if(ex is UserNotFoundException )
                {
                    return NotFound(new { message = ex.Message });
                }
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Updates the user profile.
        /// </summary>
        /// <param name="request">The update user profile request.</param>
        /// <returns>The updated user profile.</returns>
        [HttpPut("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var profile = await _userService.UpdateUserProfileAsync(new Guid(userId!), request);
                return Ok(profile);
            }
            catch (Exception ex)
            {

                if (ex is UserNotFoundException)
                {
                    return NotFound(new { message = ex.Message });
                }
                return StatusCode(500, ex.Message);
            }
        }
    }
}
