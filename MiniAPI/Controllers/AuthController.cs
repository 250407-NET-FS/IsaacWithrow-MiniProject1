

using Microsoft.AspNetCore.Mvc;
using MiniAPI.Models;
using MiniAPI.Services.Auth.Commands;
using MiniAPI.Services.Auth.Queries;

namespace MiniAPI.Controllers;

public class AuthController : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterDTO dto, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new Register.Command { Dto = dto }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync([FromBody] LoginDTO dto, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new Login.Query { Dto = dto }, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}