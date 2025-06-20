using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Application.DTOs;
using UserManagementSystem.Application.IServices;
using UserManagementSystem.Domain.IRepositories;

namespace UserManagementSystem.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IPermissionService _permissionService;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _permissionService = permissionService;
        }

        public async Task<AuthResultDto> AuthenticateAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.Username);

            if (user == null || loginDto.Password != user.PasswordHash)
            {
                return new AuthResultDto { Succeeded = false, Errors = new List<string> { "Invalid credentials" } };
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var userPermissions = await _permissionService.GetUserPermissionsAsync(user.Id);
            foreach (var permission in userPermissions)
            {
                claims.Add(new Claim("Permission", permission));
            }

            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is not configured."));
            double expiryMinutes = double.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResultDto
            {
                Succeeded = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
