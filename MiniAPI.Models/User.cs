using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace MiniAPI.Models;

public class User : IdentityUser<Guid>
{
    //All of our previous methods are already built in due to the inheritance.

    [JsonIgnore]
    public List<Purchase>? Purchases { get; set; }

    [Precision(18, 2)]
    public decimal Wallet { get; set; } = 0.0m;

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = "";
}