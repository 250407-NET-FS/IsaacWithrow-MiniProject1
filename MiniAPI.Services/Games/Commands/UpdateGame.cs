

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Games.Commands;

public class UpdateGame
{
    public class Command : IRequest<Guid>
    {
        public required Guid Id { get; set; }

        public required GameUpdateDTO Dto { get; set; }

        public required Guid UserId { get; set; }
    }

    public class Handler(MiniAPIContext context, IMapper mapper) : IRequestHandler<Command, Guid>
    {
        public async Task<Guid> Handle(Command request, CancellationToken ct)
        {
            Game? game = await context.Games.FirstOrDefaultAsync(g => g.GameID == request.Id,
             ct) ?? throw new Exception("Game does not exist.");

            if (game.OwnerID != request.UserId)
            {
                throw new Exception("Only the owner can modify game.");
            }

            mapper.Map(request.Dto, game);

            await context.SaveChangesAsync(ct);
            return game.GameID;
        }
    }
}