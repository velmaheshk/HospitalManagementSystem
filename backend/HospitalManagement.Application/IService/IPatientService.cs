using HospitalManagement.Application.DTO;
using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Application.IService
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientResponseDTO>> GetAllAsync();

        Task<PatientResponseDTO?> GetByIdAsync(int id);

        Task<PatientResponseDTO> CreateAsync(
            PatientCreateDTO dto);

        Task<PatientResponseDTO?> UpdateAsync(
            int id,
            PatientUpdateDTO dto);

        Task<bool> DeleteAsync(int id);
    }
}
