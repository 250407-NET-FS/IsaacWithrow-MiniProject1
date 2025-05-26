using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Users.Commands;

public class UpdateUser
{
    public class Command : IRequest<User>
    {
        public required Guid UserId { get; set; }

        public required UserUpdateDTO Dto { get; set; }

    }

    public class Handler : IRequestHandler<Command, User>
    {
        private readonly UserManager<User> _userManager;

        public Handler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> Handle(Command request, CancellationToken ct)
        {
            User? user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
            {
                throw new Exception("User does not exist.");
            }

            if (request.Dto.Email is not null) user.Email = request.Dto.Email;
            if (request.Dto.FirstName is not null) user.FirstName = request.Dto.FirstName;
            if (request.Dto.LastName is not null) user.LastName = request.Dto.LastName;
            if (request.Dto.Password is not null) await _userManager.AddPasswordAsync(user, request.Dto.Password);

            await _userManager.UpdateAsync(user);
            return user;
        }
    }
}