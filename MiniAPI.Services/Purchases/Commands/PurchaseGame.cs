using System;
using MiniAPI.Models;
using Microsoft.EntityFrameworkCore;
using MiniAPI.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace MiniAPI.Services.Purchases.Commands;

public class PurchaseGame
{
    public class Command : IRequest<Purchase>
    {
        public required Guid GameID { get; set; }

        public required User User { get; set; }
    }

    public class Handler : IRequestHandler<Command, Purchase>
    {
        private readonly UserManager<User> _userManager;

        private readonly MiniAPIContext _context;

        public Handler(UserManager<User> userManager, MiniAPIContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Purchase> Handle(Command request, CancellationToken ct)
        {
            //retrieve game by game id
            Game? game = await _context.Games.FirstOrDefaultAsync(g => g.GameID == request.GameID,
             ct) ?? throw new Exception("Game does not exist.");

            // check if ownerId matches userId
            if (game.OwnerID == request.User.Id)
            {
                throw new Exception("Owner cannot purchase own game.");
            }

            //check if game was already purchased by user
            Purchase? purchase = await _context.Purchases.FirstOrDefaultAsync(
                p => p.GameID == game.GameID && p.UserID == request.User.Id
            );
            if (purchase is not null)
            {
                throw new Exception("User already owns game.");
            }
            // check if user has enough funds
            if (game.Price > request.User.Wallet)
            {
                throw new Exception("User does not have enough funds.");
            }
            request.User.Wallet -= game.Price;

            Purchase p = new();
            p.Amount = game.Price;
            p.GameID = game.GameID;
            p.UserID = request.User.Id;
            // save game to database
            _context.Purchases.Add(p);
            await _context.SaveChangesAsync(ct);
            await _userManager.UpdateAsync(request.User);
            return p;
        }
    }
}