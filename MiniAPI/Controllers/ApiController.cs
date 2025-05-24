using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiniAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private IMediator? _mediator;

        protected IMediator Mediator => 
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
                ?? throw new InvalidOperationException("IMediator service is unavailable");
    }
}