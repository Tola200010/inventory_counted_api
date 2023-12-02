using InventoryCounting.Data;
using InventoryCounting.DTOs;
using InventoryCounting.Services;
using InventoryCounting.Services.Contract;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
});
builder.Services.AddScoped<IInventoryCountingService, InventoryCountingService>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseHttpLogging();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapGet("/inventory", async (IInventoryCountingService inventoryCountingService) =>
{
    return Results.Ok(await inventoryCountingService.GetAllInventoryCountedAsync());
});
app.MapPost("/inventory/counting/multi", async (List<CountingInventoryRequest> request, IInventoryCountingService inventoryCountingService) =>
{
    List<InventoryCountingDto> inventoryCountingDtos = request.Select(x =>
    {
        var inventory = new InventoryCountingDto
        {
            ItemCode = x.ItemCode,
            Quantity = x.Quantity
        };
        return inventory;
    }).ToList();
    bool isSuccess = await inventoryCountingService.AddNewInventoryCountingAsync(inventoryCountingDtos);
    return isSuccess ? Results.Ok() : Results.BadRequest();
});
app.MapPost("/inventory/ ", async (CountingInventoryRequest request, IInventoryCountingService inventoryCountingService) =>
{
    var inventory = new InventoryCountingDto
    {
        ItemCode = request.ItemCode,
        Quantity = request.Quantity
    };
    bool isSuccess = await inventoryCountingService.AddNewInventoryCountingAsync(inventory);
    return isSuccess ? Results.Ok() : Results.BadRequest();
});
app.Run();
