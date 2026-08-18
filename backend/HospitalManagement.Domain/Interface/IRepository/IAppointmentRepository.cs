using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interface.IRepository
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetByIdAsync(int appointmentId);

        Task<List<Appointment>> GetAllAsync();

        Task<List<Appointment>> GetByPatientIdAsync(int patientId);

        Task<List<Appointment>> GetByDoctorIdAsync(int doctorId);

        Task<List<Appointment>> GetByDoctorAndDateAsync(
            int doctorId,
            DateTime appointmentDate);

        Task<bool> PatientExistsAsync(int patientId);

        Task<bool> DoctorExistsAsync(int doctorId);

        Task<List<DoctorAvailability>> GetDoctorAvailabilityAsync(
            int doctorId,
            string dayOfWeek);

        Task<bool> IsSlotAlreadyBookedAsync(
            int doctorId,
            DateTime appointmentDate,
            string timeSlot,
            int? excludeAppointmentId = null);

        Task AddAsync(Appointment appointment);

        Task UpdateAsync(Appointment appointment);

        Task SaveChangesAsync();
    }
}