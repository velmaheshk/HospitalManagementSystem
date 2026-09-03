using HospitalManagement.Application.DTOs.Pharmacy;
using HospitalManagement.Application.Interfaces;
//using HospitalManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicineController : ControllerBase
{
	private readonly IMedicineService _medicineService;

	public MedicineController(IMedicineService medicineService)
	{
		_medicineService = medicineService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var medicines = await _medicineService.GetAllAsync();

		return Ok(medicines);
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetById(int id)
	{
		var medicine = await _medicineService.GetByIdAsync(id);

		if (medicine == null)
			return NotFound(new
			{
				message = "Medicine not found."
			});

		return Ok(medicine);
	}

	[HttpPost]
	public async Task<IActionResult> Create(
		[FromBody] CreateMedicineDto dto)
	{
		try
		{
			var medicine = await _medicineService.CreateAsync(dto);

			return CreatedAtAction(
				nameof(GetById),
				new { id = medicine.MedicineId },
				medicine);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new
			{
				message = ex.Message
			});
		}
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(
		int id,
		[FromBody] UpdateMedicineDto dto)
	{
		try
		{
			var result = await _medicineService.UpdateAsync(id, dto);

			if (!result)
				return NotFound(new
				{
					message = "Medicine not found."
				});

			return Ok(new
			{
				message = "Medicine updated successfully."
			});
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new
			{
				message = ex.Message
			});
		}
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _medicineService.DeleteAsync(id);

		if (!result)
			return NotFound(new
			{
				message = "Medicine not found."
			});

		return Ok(new
		{
			message = "Medicine deleted successfully."
		});
	}
}