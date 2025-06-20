using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Application.IServices;
using UserManagementSystem.Infrastructure.Data;

namespace UserManagementSystem.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _context;

        public PermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HashSet<string>> GetUserPermissionsAsync(int userId)
        {
            var permissions = new HashSet<string>();

            var user = await _context.Users
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return permissions;
            }

            foreach (var userPermission in user.UserPermissions)
            {
                permissions.Add(userPermission.Permission.PermissionName);
            }
            return permissions;
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        {
            var userPermissions = await GetUserPermissionsAsync(userId);
            return userPermissions.Contains(permissionName);
        }
    }
}
