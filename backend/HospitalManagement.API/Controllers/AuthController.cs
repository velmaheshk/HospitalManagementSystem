using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.DTOs.Auth;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagement.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>Log in with username and password. Returns a short-lived access token + a refresh token.</summary>
        /// <response code="200">Login succeeded.</response>
        /// <response code="401">Username or password is incorrect, or the account is disabled.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _authService.LoginAsync(dto);
                _logger.LogInformation("User {Username} logged in", dto.Email);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Failed login attempt for {Username}", dto.Email);
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>Patient self-registration. Creates a Patient-role User plus a minimal Patient profile.</summary>
        /// <response code="201">Account created — returns tokens so the patient is signed in immediately.</response>
        /// <response code="400">Username/email already taken, or validation failed.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _authService.RegisterAsync(dto);
                _logger.LogInformation("New patient account registered: {Username}", dto.Email);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>Exchange a still-valid refresh token for a new access token (rotates the refresh token too).</summary>
        /// <response code="200">Returns a new token pair.</response>
        /// <response code="401">The refresh token is invalid, expired, or already revoked.</response>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(dto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>Revoke a refresh token (client should also discard its stored access token).</summary>
        /// <response code="204">Logged out (idempotent — also returns 204 if the token was already invalid).</response>
        [HttpPost("logout")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto dto)
        {
            await _authService.LogoutAsync(dto.RefreshToken);
            return NoContent();
        }

        /// <summary>Check whether an email address is already registered (e.g. for live registration-form validation).</summary>
        /// <response code="200">Returns true/false.</response>
        [HttpGet("email-exists/{email}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            var exists = await _authService.EmailExistsAsync(email);
            return Ok(exists);
        }

        /// <summary>Change the logged-in user's own password.</summary>
        /// <response code="204">Password changed successfully.</response>
        /// <response code="400">Current password was incorrect.</response>
        [HttpPut("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var success = await _authService.ChangePasswordAsync(userId, dto);

            if (!success)
                return BadRequest(new { message = "Current password is incorrect." });

            return NoContent();
        }
    }
}
