using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 

namespace HospitalManagement.API.Controllers
{
    [Route("api/doctors")]
    [ApiController]
   // [Authorize] // all endpoints require a valid JWT by default; overridden per-action below
    [Produces("application/json")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly ILogger<DoctorsController> _logger;

        public DoctorsController(IDoctorService doctorService, ILogger<DoctorsController> logger)
        {
            _doctorService = doctorService;
            _logger = logger;
        }

        [HttpGet]
      //  [AllowAnonymous] // doctor directory is public so patients can browse before logging in
        [ProducesResponseType(typeof(List<DoctorDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DoctorDto>>> GetAll()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(doctors);
        }

        [HttpGet("{id:int}")]
        //[AllowAnonymous]
        [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DoctorDto>> GetById(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor is null)
                return NotFound(new { message = $"Doctor with id {id} was not found." });

            return Ok(doctor);
        }

        [HttpPost]
      //  [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DoctorDto>> Create(  CreateDoctorRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _doctorService.CreateAsync(request);
                _logger.LogInformation("Doctor {DoctorId} created by {User}", created.DoctorId, User.Identity?.Name);
                return CreatedAtAction(nameof(GetById), new { id = created.DoctorId }, created);
            }
            catch (InvalidOperationException ex)
            {
                // e.g. duplicate username — service layer throws, middleware would also catch this,
                // but we handle it locally too so the response shape matches other 400s exactly.
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
      //  [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _doctorService.UpdateAsync(id, request);
            if (!updated)
                return NotFound(new { message = $"Doctor with id {id} was not found." });

            _logger.LogInformation("Doctor {DoctorId} updated by {User}", id, User.Identity?.Name);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
      //  [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _doctorService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Doctor with id {id} was not found." });

            _logger.LogWarning("Doctor {DoctorId} deleted by {User}", id, User.Identity?.Name);
            return NoContent();
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<DoctorDashboardDto>> GetDashboard()
        {
            return Ok(await _doctorService.GetDashboardAsync());
        }
    }
}
