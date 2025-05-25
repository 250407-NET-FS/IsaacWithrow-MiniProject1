using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniAPI.Data;
using MiniAPI.Models;

namespace MiniAPI.Services.Auth.Commands;

public class Register
{
    public class Command : IRequest<string>
    {
        public required RegisterDTO Dto { get; set; }
    }

    public class Handler(UserManager<User> _userManager, IMapper mapper) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken ct)
        {
            // check if email exists
            var check = await _userManager.FindByEmailAsync(request.Dto.Email!);
            if (check is not null)
            {
                throw new Exception("Email already exists.");
            }
            User user = new();
            mapper.Map(request.Dto, user);
            user.UserName = request.Dto.Email;
            var result = await _userManager.CreateAsync(user, request.Dto.Password!);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"{error.Code} : {error.Description}");
                }
            }
            return "Registration Successful";
        }
    }
}