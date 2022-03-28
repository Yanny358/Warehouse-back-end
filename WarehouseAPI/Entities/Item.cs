using System.ComponentModel.DataAnnotations;

namespace WarehouseAPI.Entities;

public class Item
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "The field with name {0} is required")]
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;
    
    public string Image { get; set; } = String.Empty;
    
    public string? SerialNumber { get; set; }

    public int StorageId { get; set; }
    public Storage? Storages { get; set; }
}