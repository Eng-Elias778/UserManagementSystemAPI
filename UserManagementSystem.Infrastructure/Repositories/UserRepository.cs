using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Domain.IRepositories;
using UserManagementSystem.Infrastructure.Data;

namespace UserManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(u => u.Id == id) ?? new User();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var userQuery = _context.Users
                .Where(u => u.UserName == username)
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission);

            var user = await userQuery.FirstOrDefaultAsync() ?? new User();
            return user;

        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            _context.Users.Entry(user).Collection(u => u.UserPermissions).IsModified = true;
            foreach (var up in user.UserPermissions)
            {
                _context.Entry(up).State = _context.UserPermissions.Local.Any(e => e.UserId == up.UserId && e.PermissionId == up.PermissionId) ? EntityState.Unchanged : EntityState.Added;
            }
        }
    }
}
