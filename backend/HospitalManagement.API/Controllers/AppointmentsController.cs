using HospitalManagement.Application.DTO;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(
            IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // POST: api/Appointment
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateAppointmentDto dto)
        {
            try
            {
                var result =
                    await _appointmentService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.AppointmentId },
                    result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // GET: api/Appointment/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result =
                await _appointmentService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = $"Appointment with ID {id} was not found."
                });
            }

            return Ok(result);
        }

        // GET: api/Appointment
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result =
                await _appointmentService.GetAllAsync();

            return Ok(result);
        }

        // GET: api/Appointment/patient/1
        [HttpGet("patient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(
            int patientId)
        {
            var result =
                await _appointmentService
                    .GetByPatientIdAsync(patientId);

            return Ok(result);
        }

        // GET: api/Appointment/doctor/1
        [HttpGet("doctor/{doctorId:int}")]
        public async Task<IActionResult> GetByDoctor(
            int doctorId)
        {
            var result =
                await _appointmentService
                    .GetByDoctorIdAsync(doctorId);

            return Ok(result);
        }

        // GET: api/Appointment/doctor/1/date/2026-08-15
        [HttpGet("doctor/{doctorId:int}/date/{date:datetime}")]
        public async Task<IActionResult> GetByDoctorAndDate(
            int doctorId,
            DateTime date)
        {
            var result =
                await _appointmentService
                    .GetByDoctorAndDateAsync(
                        doctorId,
                        date);

            return Ok(result);
        }

        // PUT: api/Appointment/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateAppointmentDto dto)
        {
            try
            {
                var result =
                    await _appointmentService
                        .UpdateAsync(id, dto);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message =
                            $"Appointment with ID {id} was not found."
                    });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // PATCH: api/Appointment/1/cancel
        [HttpPatch("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var result =
                    await _appointmentService
                        .CancelAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        message =
                            $"Appointment with ID {id} was not found."
                    });
                }

                return Ok(new
                {
                    message = "Appointment cancelled successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // PATCH: api/Appointment/1/complete
        [HttpPatch("{id:int}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                var result =
                    await _appointmentService
                        .CompleteAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        message =
                            $"Appointment with ID {id} was not found."
                    });
                }

                return Ok(new
                {
                    message = "Appointment completed successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}