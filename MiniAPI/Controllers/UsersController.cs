using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniAPI.Models;
using MiniAPI.Services.Users.Commands;
using MiniAPI.Services.Users.Queries;

namespace MiniAPI.Controllers;

public class UsersController : ApiController
{
    private readonly UserManager<User> _userManager;

    public UsersController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync(CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new GetAllUsersAdmin.Query { }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/{id}")]
    public async Task<ActionResult<User>> GetUserAdminAsync([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new GetUser.Query { UserId = id }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("admin/{id}")]
    public async Task<ActionResult<bool>> DeleteUserAdminAsync([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new DeleteUser.Command { UserId = id }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<User>> GetUserAsync(CancellationToken ct)
    {
        try
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await Mediator.Send(new GetUser.Query { UserId = user!.Id }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPatch]
    public async Task<ActionResult<User>> UpdateUserAsync([FromBody] UserUpdateDTO dto, CancellationToken ct)
    {
        try
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await Mediator.Send(new UpdateUser.Command { UserId = user!.Id, Dto = dto }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteUserAsync(CancellationToken ct)
    {
        try
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await Mediator.Send(new DeleteUser.Command { UserId = user!.Id }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPatch("wallet")]
    public async Task<ActionResult<decimal>> AddFundsAsync([FromBody] decimal funds, CancellationToken ct)
    {
        try
        {
            User? user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await Mediator.Send(new AddUserFunds.Command{UserId = user!.Id, Funds = funds}, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}