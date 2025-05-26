

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Games.Commands;

public class DeleteGame
{
    public class Command : IRequest<Guid>
    {
        public required Guid Id { get; set; }

        public required Guid UserId { get; set; }
    }

    public class Handler(MiniAPIContext context) : IRequestHandler<Command, Guid>
    {
        public async Task<Guid> Handle(Command request, CancellationToken ct)
        {
            Game? game = await context.Games.FirstOrDefaultAsync(g => g.GameID == request.Id,
             ct) ?? throw new Exception("Game does not exist.");
            if (game.OwnerID != request.UserId)
            {
                throw new Exception("Only the owner can modify game.");
            }
            context.Games.Remove(game);
            await context.SaveChangesAsync(ct);
            return request.Id;
        }
    }
}