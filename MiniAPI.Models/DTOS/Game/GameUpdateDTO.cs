


using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MiniAPI.Models;

public class GameUpdateDTO
{
    [Required]
    [StringLength(50)]
    public string? Title { get; set; }

    [Required]
    [StringLength(50)]
    public string? Publisher { get; set; }

    [Required]
    [Precision(18, 2)]
    public decimal Price { get; set; }
}