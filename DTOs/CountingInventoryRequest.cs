using System.ComponentModel.DataAnnotations;

namespace InventoryCounting.DTOs;

public record CountingInventoryRequest([Required(ErrorMessage = "Item code is required.")] string? ItemCode, [Required(ErrorMessage = "Quantity is required.")] int Quantity);