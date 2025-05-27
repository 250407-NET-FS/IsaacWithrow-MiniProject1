using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniAPI.Models;
using MiniAPI.Services.Purchases.Commands;
using MiniAPI.Services.Purchases.Queries;

namespace MiniAPI.Controllers;

public class PurchasesController : ApiController
{
    private readonly UserManager<User> _userManager;

    public PurchasesController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<ActionResult<List<Purchase>>> GetAllPurchasesAsync(CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new GetAllPurchases.Query { }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPost("game/{id}")]
    public async Task<ActionResult<Purchase>> PurchaseGameAsync([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await Mediator.Send(new PurchaseGame.Command { GameID = id, User = user! }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<Purchase>>> GetAllUserPurchases(CancellationToken ct)
    {
        try
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await Mediator.Send(new GetAllUserPurchases.Query { User = user! }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpGet("game/{id}")]
    public async Task<ActionResult<List<Purchase>>> GetAllGamePurchases([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new GetAllGamePurchases.Query { GameID = id }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpDelete("game/{id}")]
    public async Task<ActionResult<bool>> RefundGameAsync([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await Mediator.Send(new RefundGame.Command { GameID = id, User = user! }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}