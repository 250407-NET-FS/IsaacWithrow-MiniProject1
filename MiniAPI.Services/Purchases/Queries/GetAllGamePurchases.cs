

using System.Reflection.Metadata;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Purchases.Queries;

public class GetAllGamePurchases
{
    public class Query : IRequest<List<Purchase>>
    {
        public required Guid GameID { get; set; }
    }

    public class Handler : IRequestHandler<Query, List<Purchase>>
    {
        public readonly MiniAPIContext _context;

        public Handler(MiniAPIContext context)
        {
            _context = context;
        } 
        public async Task<List<Purchase>> Handle(Query request, CancellationToken ct)
        {
            return await _context.Purchases.Where(p => p.GameID == request.GameID).ToListAsync(ct);
        }
    }
}