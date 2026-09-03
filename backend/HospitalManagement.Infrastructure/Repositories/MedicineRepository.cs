using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories;

public class MedicineRepository : IMedicineRepository
{
	private readonly ApplicationDbContext _context;

	public MedicineRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Medicine>> GetAllAsync()
	{
		return await _context.Medicines
			.AsNoTracking()
			.ToListAsync();
	}

	public async Task<Medicine?> GetByIdAsync(int id)
	{
		return await _context.Medicines
			.FirstOrDefaultAsync(x => x.MedicineId == id);
	}

	public async Task<Medicine> AddAsync(Medicine medicine)
	{
		await _context.Medicines.AddAsync(medicine);
		await _context.SaveChangesAsync();

		return medicine;
	}

	public async Task UpdateAsync(Medicine medicine)
	{
		_context.Medicines.Update(medicine);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(Medicine medicine)
	{
		_context.Medicines.Remove(medicine);
		await _context.SaveChangesAsync();
	}

	public async Task<bool> ExistsAsync(int id)
	{
		return await _context.Medicines
			.AnyAsync(x => x.MedicineId == id);
	}
}