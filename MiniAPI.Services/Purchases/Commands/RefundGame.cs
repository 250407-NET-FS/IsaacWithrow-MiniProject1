using System;
using MiniAPI.Models;
using Microsoft.EntityFrameworkCore;
using MiniAPI.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace MiniAPI.Services.Purchases.Commands;

public class RefundGame
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
            //retrieve user purchases
            request.User.Purchases = await _context.Purchases.Where(p => p.UserID == request.User.Id).ToListAsync(ct);

            Purchase? p = request.User.Purchases.FirstOrDefault(p => p.GameID == request.GameID);

            if (p is null)
            {
                throw new Exception("Purchase does not exist.");
            }

            request.User.Wallet += p.Amount;
            request.User.Purchases.Remove(p);
            await _userManager.UpdateAsync(request.User);
            _context.Purchases.Remove(p);
            await _context.SaveChangesAsync(ct);
            return p;
        }
    }
}