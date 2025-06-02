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
    public string Title { get; set; } = "";

    [Required]
    [Precision(18, 2)]
    public decimal Price { get; set; }

    [Required]
    [StringLength(50)]
    public string Publisher { get; set; } = "";

    [Required]
    [ForeignKey("OwnerID")]
    public Guid OwnerID { get; set; }

    // Image stored as byte array
    [Required]
    public byte[] ImageData { get; set; } = [];

    [Required]
    [StringLength(200)]

    public string ImageMimeType { get; set; } = ""; // e.g. image/jpeg

    [JsonIgnore]
    public List<Purchase> Purchases { get; set; } = [];

    public DateTime PublishDate { get; set; } = DateTime.Now;


    public Game()
    {

    }

    public Game(GameCreateDTO dto)
    {
        Title = dto.Title!;
        Price = dto.Price;
        Publisher = dto.Publisher!;
        ImageData = dto.ImageData;
        ImageMimeType = dto.ImageMimeType;
    }

}