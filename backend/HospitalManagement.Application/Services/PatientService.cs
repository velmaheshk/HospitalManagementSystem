using HospitalManagement.Application.DTO;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.IRepository;

namespace HospitalManagement.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(
            IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<PatientResponseDTO>> GetAllAsync()
        {
            var patients =
                await _patientRepository.GetAllAsync();

            return patients.Select(MapToResponse);
        }

        public async Task<PatientResponseDTO?> GetByIdAsync(int id)
        {
            var patient =
                await _patientRepository.GetByIdAsync(id);

            if (patient == null)
                return null;

            return MapToResponse(patient);
        }

        public async Task<PatientResponseDTO> CreateAsync(
            PatientCreateDTO dto)
        {
            var patient = new Patient
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                DOB = dto.DOB,
                Gender = dto.Gender,
                Address = dto.Address,
                BloodGroup = dto.BloodGroup,
                EmergencyContactName =
                    dto.EmergencyContactName,
                EmergencyContactPhone =
                    dto.EmergencyContactPhone,
                CreatedAt = DateTime.UtcNow
            };

            var createdPatient =
                await _patientRepository.CreateAsync(patient);

            return MapToResponse(createdPatient);
        }

        public async Task<PatientResponseDTO?> UpdateAsync(int id,PatientUpdateDTO dto)
        {
            var patient = new Patient
            {
                FullName = dto.FullName,
                DOB = dto.DOB,
                Gender = dto.Gender,
                Address = dto.Address,
                BloodGroup = dto.BloodGroup,
                EmergencyContactName = dto.EmergencyContactName,
                EmergencyContactPhone = dto.EmergencyContactPhone
            };

            var updatedPatient =
                await _patientRepository.UpdateAsync(id, patient);

            if (updatedPatient == null)
                return null;

            return MapToResponse(updatedPatient);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _patientRepository.DeleteAsync(id);
        }

        private static PatientResponseDTO MapToResponse(
            Patient patient)
        {
            return new PatientResponseDTO
            {
                PatientId = patient.PatientId,
                UserId = patient.UserId,
                FullName = patient.FullName,
                DOB = patient.DOB,
                Gender = patient.Gender,
                Address = patient.Address,
                BloodGroup = patient.BloodGroup,
                EmergencyContactName =
                    patient.EmergencyContactName,
                EmergencyContactPhone =
                    patient.EmergencyContactPhone,
                CreatedAt = patient.CreatedAt
            };
        }
    }
}