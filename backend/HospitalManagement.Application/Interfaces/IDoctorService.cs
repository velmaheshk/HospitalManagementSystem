using HospitalManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Application.Interfaces
{
       public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<DoctorDto> CreateAsync(CreateDoctorRequest request);
        Task<bool> UpdateAsync(int id, UpdateDoctorRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
