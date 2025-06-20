using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementSystem.Application.DTOs
{
    public class AuthResultDto
    {
        public string Token { get; set; }
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
    }
}
