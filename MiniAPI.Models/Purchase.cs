using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiniAPI.Models;

public class Purchase
{
    [Key]
    public Guid PurchaseID { get; set; } = Guid.NewGuid();

    [Required]
    [ForeignKey("UserID")]
    public Guid UserID { get; set; }

    [Required]
    [ForeignKey("GameID")]
    public Guid GameID { get; set; }

    [Required]
    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public DateTime PurchaseDate { get; set; } = DateTime.Now;
}