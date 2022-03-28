using System.ComponentModel.DataAnnotations;

namespace WarehouseAPI.Entities;

public class Storage
{
    public int Id { get; set; }
    [Required(ErrorMessage = "The field with name {0} is required")]
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;
    
    public ICollection<Item>? Items { get; set; }
}