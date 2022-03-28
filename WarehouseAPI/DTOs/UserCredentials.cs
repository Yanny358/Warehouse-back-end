using System.ComponentModel.DataAnnotations;

namespace WarehouseAPI.DTOs;

public class UserCredentials
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = String.Empty;
    [Required]
    public string Password { get; set; } = String.Empty;
}