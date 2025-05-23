using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MiniAPI.Models;

[Table("Games")]
public class Game
{
    [Key]
    public Guid GameID { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(50)]
    public string? Title { get; set; }

    [Required]
    [Precision(18, 2)]
    public decimal Price { get; set; }

    [Required]
    [StringLength(50)]
    public string? Publisher { get; set; }

    [JsonIgnore]
    public List<Purchase>? Purchases { get; set; }

    public DateTime PublishDate { get; set; } = DateTime.Now;


    public Game()
    {

    }

    public Game(GameCreateDTO dto)
    {
        Title = dto.Title;
        Price = dto.Price;
        Publisher = dto.Publisher;
    }

}