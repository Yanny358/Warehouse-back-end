using System.ComponentModel.DataAnnotations;

namespace WarehouseAPI.DTOs;

public class StorageCreationDTO
{
    [Required(ErrorMessage = "The field with name {0} is required")]
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;
}