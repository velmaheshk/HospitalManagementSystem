using HospitalManagement.Application.DTO;
using HospitalManagement.Application.DTOs.Appointment;

namespace HospitalManagement.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentResponseDto> CreateAsync(
            CreateAppointmentDto dto);

        Task<AppointmentResponseDto?> GetByIdAsync(
            int appointmentId);

        Task<List<AppointmentResponseDto>> GetAllAsync();

        Task<List<AppointmentResponseDto>> GetByPatientIdAsync(
            int patientId);

        Task<List<AppointmentResponseDto>> GetByDoctorIdAsync(
            int doctorId);

        Task<List<AppointmentResponseDto>> GetByDoctorAndDateAsync(
            int doctorId,
            DateTime appointmentDate);

        Task<AppointmentResponseDto?> UpdateAsync(
            int appointmentId,
            UpdateAppointmentDto dto);

        Task<bool> CancelAsync(int appointmentId);

        Task<bool> CompleteAsync(int appointmentId);
    }
}