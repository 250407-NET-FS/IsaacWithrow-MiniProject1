using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Users.Queries;

public class GetUser
{
    public class Query : IRequest<User>
    {
        public required Guid UserId { get; set; }
    }

    public class Handler : IRequestHandler<Query, User>
    {
        private readonly UserManager<User> _userManager;

        public Handler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> Handle(Query request, CancellationToken ct)
        {
            User? user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
            {
                throw new Exception("User does not exist.");
            }
            return user;
        }
    }
}