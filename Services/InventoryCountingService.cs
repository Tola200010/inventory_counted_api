using Dapper;
using InventoryCounting.DTOs;
using InventoryCounting.Services.Contract;
using Microsoft.Data.SqlClient;

namespace InventoryCounting.Services;

public class InventoryCountingService : IInventoryCountingService
{
    private string _connectionString { get; }
    public InventoryCountingService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")!;
    }
    public async Task<bool> AddNewInventoryCountingAsync(List<InventoryCountingDto> inventoryCounted)
    {
        using var connection = new SqlConnection(_connectionString);
        if (connection.State == System.Data.ConnectionState.Closed)
            await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();
        int rowAffected = 0;
        try
        {
            const string sql = "INSERT INTO INVENTORY_COUNTING(ITEM_CODE,QUANTITY,CREATED_AT) VALUES(@ITEM_CODE,@QUANTITY,@CREATED_AT)";
            foreach (var item in inventoryCounted)
            {
                object[] parameters = { new { ITEM_CODE = item.ItemCode, QUANTITY = item.Quantity, CREATED_AT = DateTime.Now } };
                rowAffected = await connection.ExecuteAsync(sql, parameters,transaction);
            }
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }
        return rowAffected > 0;
    }

    public async Task<bool> AddNewInventoryCountingAsync(InventoryCountingDto inventoryCounted)
    {
        using var connection = new SqlConnection(_connectionString);
        if (connection.State == System.Data.ConnectionState.Closed)
            await connection.OpenAsync();
        int rowAffected = 0;
        try
        {
            const string sql = "INSERT INTO INVENTORY_COUNTING(ITEM_CODE,QUANTITY,CREATED_AT) VALUES(@ITEM_CODE,@QUANTITY,@CREATED_AT)";
            object[] parameters = { new { ITEM_CODE = inventoryCounted.ItemCode, QUANTITY = inventoryCounted.Quantity, CREATED_AT = DateTime.Now } };
            rowAffected = await connection.ExecuteAsync(sql, parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }
        return rowAffected > 0;
    }

    public async Task<bool> DeleteInventoryCountedAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        if (connection.State == System.Data.ConnectionState.Closed)
            await connection.OpenAsync();
        int rowAffected = 0;
        try
        {
            const string sql = @"DELETE INVENTORY_COUNTING WHERE ID = @ID";
            object[] parameters = { new { ID = id } };
            rowAffected = await connection.ExecuteAsync(sql,parameters);
        }catch(Exception e){
            Console.WriteLine(e.Message);
            throw;
        }
        finally{
            await connection.CloseAsync();
        }
        return rowAffected >0;
    }

    public async Task<IEnumerable<InventoryCountingDto>> GetAllInventoryCountedAsync()
    {
        IEnumerable<InventoryCountingDto> inventoryCountingList;
        using var connection = new SqlConnection(_connectionString);
        if (connection.State == System.Data.ConnectionState.Closed)
            await connection.OpenAsync();
        try
        {
            const string sql = "SELECT ID Id,ITEM_CODE ItemCode,QUANTITY Quantity,CREATED_AT CreatedDate FROM INVENTORY_COUNTING;";
            inventoryCountingList = await connection.QueryAsync<InventoryCountingDto>(sql);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }
        return inventoryCountingList;
    }

    public async Task<bool> UpdateInventoryCountedAsync(InventoryCountingDto inventoryCounted)
    {
        using var connection = new SqlConnection(_connectionString);
        if (connection.State == System.Data.ConnectionState.Closed)
            await connection.OpenAsync();
        int rowAffected = 0;
        try
        {
            const string sql = @"UPDATE INVENTORY_COUNTING SET ITEM_CODE = @ITEM_CODE,QUANTITY = @QUANTITY WHERE ID = @ID";
            object[] parameters = { new { ITEM_CODE = inventoryCounted.ItemCode, Email = inventoryCounted.Quantity, ID = inventoryCounted.Id } };
            rowAffected = await connection.ExecuteAsync(sql,parameters);
        }catch(Exception e){
            Console.WriteLine(e.Message);
            throw;
        }
        finally{
            await connection.CloseAsync();
        }
        return rowAffected >0;
    }
}