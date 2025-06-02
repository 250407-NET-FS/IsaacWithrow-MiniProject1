using MiniAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MiniAPI.Services.Games.Commands;
using MiniAPI.Services.Games.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MiniAPI.Controllers;

public class GamesController : ApiController
{
    private readonly UserManager<User> _userManager;
    public GamesController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

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

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateGameAsync([FromBody] GameCreateDTO dto, CancellationToken ct)
    {
        try
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState); // returns detailed validation error

            User? user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await Mediator.Send(new CreateGame.Command { Dto = dto, OwnerID = user!.Id }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGameAsync([FromRoute] Guid id, [FromBody] GameUpdateDTO Dto, CancellationToken ct)
    {
        try
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await Mediator.Send(new UpdateGame.Command { Id = id, Dto = Dto, UserId = user!.Id }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGameAsync([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await Mediator.Send(new DeleteGame.Command { Id = id, UserId = user!.Id }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}