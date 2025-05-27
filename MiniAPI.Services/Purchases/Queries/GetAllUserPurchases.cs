

using System.Reflection.Metadata;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Purchases.Queries;

public class GetAllUserPurchases
{
    public class Query : IRequest<List<Purchase>>
    {
        public required User User { get; set; }
    }

    public class Handler : IRequestHandler<Query, List<Purchase>>
    {
        public readonly UserManager<User> _userManager;

        public readonly MiniAPIContext _context;

        public Handler(UserManager<User> userManager, MiniAPIContext context)
        {
            _userManager = userManager;
            _context = context;
        } 
        public async Task<List<Purchase>> Handle(Query request, CancellationToken ct)
        {
            return await _context.Purchases.Where(p => p.UserID == request.User.Id).ToListAsync(ct);
        }
    }
}