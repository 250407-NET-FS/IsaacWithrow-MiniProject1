

using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Purchases.Queries;

public class GetAllPurchases
{
    public class Query : IRequest<List<Purchase>>
    {

    }

    public class Handler(MiniAPIContext context) : IRequestHandler<Query, List<Purchase>>
    {
        public async Task<List<Purchase>> Handle(Query request, CancellationToken ct) {
            return await context.Purchases.ToListAsync(ct);
        }
    }
}