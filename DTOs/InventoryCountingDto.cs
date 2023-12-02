namespace InventoryCounting.DTOs;

public class InventoryCountingDto
{
    
    public int Id { get; set; }
    public string? ItemCode { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; }
}