using MiniAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MiniAPI.Data;

public class MiniAPIContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public MiniAPIContext(DbContextOptions<MiniAPIContext> options) : base(options) { }

    public DbSet<Game> Games { get; set; }

    public DbSet<Purchase> Purchases { get; set; }
}
