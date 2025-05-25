

using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Games.Queries;

public class GetAllGames
{
    public class Query : IRequest<List<Game>>
    {

    }

    public class Handler(MiniAPIContext context) : IRequestHandler<Query, List<Game>>
    {
        public async Task<List<Game>> Handle(Query request, CancellationToken ct) {
            return await context.Games.ToListAsync(ct);
        }
    }
}