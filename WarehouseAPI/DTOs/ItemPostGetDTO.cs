using WarehouseAPI.Entities;

namespace WarehouseAPI.DTOs;

public class ItemPostGetDTO
{
    public List<StorageDTO> Storage { get; set; }
}