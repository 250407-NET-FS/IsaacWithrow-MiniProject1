using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Users.Commands;

public class AddUserFunds
{
    public class Command : IRequest<decimal>
    {
        public required Guid UserId { get; set; }

        [Precision(18, 2)]
        public required decimal Funds { get; set; }

    }

    public class Handler : IRequestHandler<Command, decimal>
    {
        private readonly UserManager<User> _userManager;

        public Handler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<decimal> Handle(Command request, CancellationToken ct)
        {
            User? user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
            {
                throw new Exception("User does not exist.");
            }
            user.Wallet += request.Funds;
            await _userManager.UpdateAsync(user);
            return user.Wallet; 
        }
    }
}