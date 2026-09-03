using System;
using System.Collections.Generic;
using System.Text;
using HospitalManagement.Application.DTOs.Pharmacy;
//namespace HospitalManagement.Application.Interfaces.Services;

namespace HospitalManagement.Application.Interfaces
{
	public interface IMedicineService
	{
		Task<IEnumerable<MedicineDto>> GetAllAsync();

		Task<MedicineDto?> GetByIdAsync(int id);

		Task<MedicineDto> CreateAsync(CreateMedicineDto dto);

		Task<bool> UpdateAsync(int id, UpdateMedicineDto dto);

		Task<bool> DeleteAsync(int id);
	}
}