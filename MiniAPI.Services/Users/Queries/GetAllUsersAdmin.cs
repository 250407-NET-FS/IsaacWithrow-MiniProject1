using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Users.Queries;

public class GetAllUsersAdmin
{
    public class Query : IRequest<List<User>>
    {

    }

    public class Handler : IRequestHandler<Query, List<User>>
    {
        private readonly UserManager<User> _userManager;

        public Handler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<User>> Handle(Query request, CancellationToken ct)
        {
            List<User> users = await _userManager.Users.ToListAsync(ct);

            return users;
        }
    }
}