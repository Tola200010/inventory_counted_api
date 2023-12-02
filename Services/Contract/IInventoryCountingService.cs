using InventoryCounting.DTOs;

namespace InventoryCounting.Services.Contract;

public interface IInventoryCountingService
{
    Task<IEnumerable<InventoryCountingDto>> GetAllInventoryCountedAsync();
    Task<bool> AddNewInventoryCountingAsync(List<InventoryCountingDto> inventoryCounted);
    Task<bool> AddNewInventoryCountingAsync(InventoryCountingDto inventoryCounted);
    Task<bool> UpdateInventoryCountedAsync(InventoryCountingDto inventoryCounted);
    Task<bool> DeleteInventoryCountedAsync(int id);
}