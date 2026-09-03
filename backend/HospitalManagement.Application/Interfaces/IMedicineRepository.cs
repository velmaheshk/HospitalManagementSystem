using System;
using System.Collections.Generic;
using System.Text;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces
{
	public interface IMedicineRepository
	{
		Task<IEnumerable<Medicine>> GetAllAsync();

		Task<Medicine?> GetByIdAsync(int id);

		Task<Medicine> AddAsync(Medicine medicine);

		Task UpdateAsync(Medicine medicine);

		Task DeleteAsync(Medicine medicine);

		Task<bool> ExistsAsync(int id);
	}
}