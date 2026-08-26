using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Application.DTOs; 

namespace HospitalManagement.API.Controllers
{
    [Route("api/users")]
    //[Authorize(Policy = "AdminOnly")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>Get all user accounts (Admin, Doctor, and Patient logins).</summary>
        /// <response code="200">Returns the list of users.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>Get a single user account by id.</summary>
        /// <response code="200">Returns the user.</response>
        /// <response code="404">No user exists with the given id.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user is null)
                return NotFound(new { message = $"User with id {id} was not found." });

            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
         //[Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<UserDto>> Create(
             CreateUserRequest request)
        {
            try
            {

                var user = await _userService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = user.UserId },
                    user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // PUT: api/User/5
        [HttpPut("{id:int}")]
      //  [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<UserDto>> Update(
            int id,
              UpdateUserRequest request)
        {
            try
            {
                var user = await _userService.UpdateAsync(id, request);

                if (user is null)
                    return NotFound(new
                    {
                        message = $"User with ID {id} was not found."
                    });

                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        /// <summary>Enable or disable a user account (disabled accounts cannot log in).</summary>
        /// <response code="204">Status updated successfully.</response>
        /// <response code="404">No user exists with the given id.</response>
        [HttpPut("{id:int}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateUserStatusRequest request)
        {
            var updated = await _userService.UpdateStatusAsync(id, request);
            if (!updated)
                return NotFound(new { message = $"User with id {id} was not found." });

            _logger.LogInformation("User {UserId} {Action} by {Admin}",
                id, request.IsActive ? "enabled" : "disabled", User.Identity?.Name);
            return NoContent();
        }

        /// <summary>Delete a user account.</summary>
        /// <response code="204">User deleted successfully.</response>
        /// <response code="404">No user exists with the given id.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"User with id {id} was not found." });

            _logger.LogWarning("User {UserId} deleted by {Admin}", id, User.Identity?.Name);
            return NoContent();
        }
    }
}

