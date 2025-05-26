using System.ComponentModel.DataAnnotations;

namespace MiniAPI.Models;

public class UserUpdateDTO
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    public string? Password { get; set; }
}