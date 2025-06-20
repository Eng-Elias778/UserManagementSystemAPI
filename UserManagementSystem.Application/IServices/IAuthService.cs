using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Application.DTOs;

namespace UserManagementSystem.Application.IServices
{
    public interface IAuthService
    {
        Task<AuthResultDto> AuthenticateAsync(LoginDto loginDto);
    }
}
