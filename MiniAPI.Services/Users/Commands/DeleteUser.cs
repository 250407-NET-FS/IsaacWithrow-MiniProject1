using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Users.Commands;

public class DeleteUser
{
    public class Command : IRequest<bool>
    {
        public required Guid UserId { get; set; }

    }

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly UserManager<User> _userManager;

        public Handler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(Command request, CancellationToken ct)
        {
            User? user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
            {
                throw new Exception("User does not exist.");
            }
            await _userManager.DeleteAsync(user);
            return true;
        }
    }
}