using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MiniAPI.Models;

public class GameCreateDTO
{
    [Required]
    [StringLength(50)]
    public string? Title { get; set; }

    [Required]
    [Precision(18, 2)]
    public decimal Price { get; set; }

    [Required]
    [StringLength(50)]
    public string? Publisher { get; set; }
}