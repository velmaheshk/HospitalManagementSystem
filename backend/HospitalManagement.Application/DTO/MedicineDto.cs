namespace HospitalManagement.Application.DTOs.Pharmacy;

public class MedicineDto
{
	public int MedicineId { get; set; }

	public string Name { get; set; } = string.Empty;

	public string Category { get; set; } = string.Empty;

	public string Manufacturer { get; set; } = string.Empty;

	public decimal UnitPrice { get; set; }

	public int StockQuantity { get; set; }

	public DateTime ExpiryDate { get; set; }

	public int ReorderLevel { get; set; }
}