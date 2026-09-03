using System;
using System.Collections.Generic;
using System.Text;
using HospitalManagement.Application.DTOs.Pharmacy;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Services
{
	public class MedicineService : IMedicineService
	{
		private readonly IMedicineRepository _repository;

		public MedicineService(IMedicineRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<MedicineDto>> GetAllAsync()
		{
			var medicines = await _repository.GetAllAsync();

			return medicines.Select(x => new MedicineDto
			{
				MedicineId = x.MedicineId,
				Name = x.Name,
				Category = x.Category,
				Manufacturer = x.Manufacturer,
				UnitPrice = x.UnitPrice,
				StockQuantity = x.StockQuantity,
				ExpiryDate = x.ExpiryDate,
				ReorderLevel = x.ReorderLevel
			});
		}

		public async Task<MedicineDto?> GetByIdAsync(int id)
		{
			var medicine = await _repository.GetByIdAsync(id);

			if (medicine == null)
				return null;

			return new MedicineDto
			{
				MedicineId = medicine.MedicineId,
				Name = medicine.Name,
				Category = medicine.Category,
				Manufacturer = medicine.Manufacturer,
				UnitPrice = medicine.UnitPrice,
				StockQuantity = medicine.StockQuantity,
				ExpiryDate = medicine.ExpiryDate,
				ReorderLevel = medicine.ReorderLevel
			};
		}

		public async Task<MedicineDto> CreateAsync(CreateMedicineDto dto)
		{
			if (dto.UnitPrice < 0)
				throw new ArgumentException("Unit price cannot be negative.");

			if (dto.StockQuantity < 0)
				throw new ArgumentException("Stock quantity cannot be negative.");

			if (dto.ExpiryDate.Date <= DateTime.UtcNow.Date)
				throw new ArgumentException("Expiry date must be a future date.");

			var medicine = new Medicine
			{
				Name = dto.Name,
				Category = dto.Category,
				Manufacturer = dto.Manufacturer,
				UnitPrice = dto.UnitPrice,
				StockQuantity = dto.StockQuantity,
				ExpiryDate = dto.ExpiryDate,
				ReorderLevel = dto.ReorderLevel
			};

			var created = await _repository.AddAsync(medicine);

			return new MedicineDto
			{
				MedicineId = created.MedicineId,
				Name = created.Name,
				Category = created.Category,
				Manufacturer = created.Manufacturer,
				UnitPrice = created.UnitPrice,
				StockQuantity = created.StockQuantity,
				ExpiryDate = created.ExpiryDate,
				ReorderLevel = created.ReorderLevel
			};
		}

		public async Task<bool> UpdateAsync(
			int id,
			UpdateMedicineDto dto)
		{
			var medicine = await _repository.GetByIdAsync(id);

			if (medicine == null)
				return false;

			if (dto.UnitPrice < 0)
				throw new ArgumentException("Unit price cannot be negative.");

			if (dto.StockQuantity < 0)
				throw new ArgumentException("Stock quantity cannot be negative.");

			medicine.Name = dto.Name;
			medicine.Category = dto.Category;
			medicine.Manufacturer = dto.Manufacturer;
			medicine.UnitPrice = dto.UnitPrice;
			medicine.StockQuantity = dto.StockQuantity;
			medicine.ExpiryDate = dto.ExpiryDate;
			medicine.ReorderLevel = dto.ReorderLevel;

			await _repository.UpdateAsync(medicine);

			return true;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var medicine = await _repository.GetByIdAsync(id);

			if (medicine == null)
				return false;

			await _repository.DeleteAsync(medicine);

			return true;
		}
	}
}
