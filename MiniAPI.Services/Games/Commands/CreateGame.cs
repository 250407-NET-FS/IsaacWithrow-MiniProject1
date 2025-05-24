using System;
using MiniAPI.Models;
using MiniAPI.Data;
using MediatR;

namespace MiniAPI.Services.Games.Commands;

public class CreateGame
{
    public class Command : IRequest<Guid>
    {
        public required GameCreateDTO Dto { get; set; }
    }

    public class Handler(MiniAPIContext context) : IRequestHandler<Command, Guid>
    {
        public async Task<Guid> Handle(Command request, CancellationToken ct)
        {
            // create game from DTO
            Game game = new(request.Dto);

            // save game to database
            context.Games.Add(request.Dto);
            await context.SaveChangesAsync(ct);
            return game.GameID;
        }
    }
}