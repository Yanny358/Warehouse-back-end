using WarehouseAPI.Entities;

namespace WarehouseAPI.DTOs;

public class ItemDTO
{
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Image { get; set; } = String.Empty;
    
    public string? SerialNumber { get; set; }

    public Storage Storage { get; set; }
}