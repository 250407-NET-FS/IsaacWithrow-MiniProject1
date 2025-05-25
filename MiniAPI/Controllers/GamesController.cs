



using MiniAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MiniAPI.Services.Games.Commands;
using MiniAPI.Services.Games.Queries;

namespace MiniAPI.Controllers;

public class GamesController : ApiController
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Game>>> GetAllGamesAsync(CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new GetAllGames.Query(), ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateGameAsync([FromBody] GameCreateDTO dto, CancellationToken ct)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Model State");
            }
            var result = await Mediator.Send(new CreateGame.Command { Dto = dto }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGameAsync([FromRoute] Guid id, [FromBody] GameUpdateDTO Dto, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new UpdateGame.Command { Id = id, Dto = Dto }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGameAsync([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new DeleteGame.Command { Id = id }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}