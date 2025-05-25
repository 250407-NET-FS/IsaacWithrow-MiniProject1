using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniAPI.Models;

namespace MiniAPI.Services.Auth.Queries;

public class Login
{
    public class Query : IRequest<string>
    {
        public required LoginDTO? Dto { get; set; }
    }

    public class Handler(UserManager<User> _userManager, IConfiguration _config) : IRequestHandler<Query, string>
    {
        public async Task<string> Handle(Query request, CancellationToken ct)
        {
            // check if email matches a user
            User? user = await _userManager.FindByEmailAsync(request.Dto!.Email!)
             ?? throw new Exception("Email does not exist.");

            // check if password matches the user
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Dto.Password!);
            if (!isPasswordValid)
            {
                throw new Exception("Password is incorrect.");
            }
            return await GenerateUserTokenAsync(user);
        }

        public async Task<string> GenerateUserTokenAsync(User user) {
            List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!)
            };
            
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_config.GetValue<double>("Jwt:ExpireDays")),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}