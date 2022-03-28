using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.Helpers;

namespace WarehouseAPI.DTOs;

public class ItemCreationDTO
{
    public string Title { get; set; } = string.Empty;
    
    public IFormFile Image { get; set; } 
    
    public string? SerialNumber { get; set; }
    
    [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
    public List<int> StorageId { get; set; }
}