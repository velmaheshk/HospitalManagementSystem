using HospitalManagement.Application.DTO;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients =
                await _patientService.GetAllAsync();

            return Ok(patients);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient =
                await _patientService.GetByIdAsync(id);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = "Patient not found"
                });
            }

            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientCreateDTO dto)
        {
            var patient =
                await _patientService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = patient.PatientId },
                patient);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] PatientUpdateDTO dto)
        {
            var patient =
                await _patientService.UpdateAsync(id, dto);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = "Patient not found"
                });
            }

            return Ok(patient);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _patientService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Patient not found"
                });
            }

            return Ok(new
            {
                message = "Patient deleted successfully"
            });
        }
    }
}