// UserManagementSystem.Infrastructure/Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Domain.Entities;
using BCrypt.Net; // Keep this for the hashing function itself, but the values will be static

namespace UserManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Call SeedData with the modelBuilder
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {

            // Seed Permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, PermissionName = "Users.Read" },
                new Permission { Id = 2, PermissionName = "Users.Create" },
                new Permission { Id = 3, PermissionName = "Users.Update" },
                new Permission { Id = 4, PermissionName = "Users.Delete" },
                new Permission { Id = 5, PermissionName = "Permissions.Read" },
                new Permission { Id = 6, PermissionName = "Permissions.Manage" }
            );

            // Seed an initial Admin user
            var adminUser = new User
            {
                Id = 1,
                UserName = "admin",
                Email = "admin@example.com",
                PasswordHash = "password123" // Use the static hash here
            };
            modelBuilder.Entity<User>().HasData(adminUser);

            // Assign ALL permissions to the initial Admin user directly
            modelBuilder.Entity<UserPermission>().HasData(
                new UserPermission { UserId = 1, PermissionId = 1 },
                new UserPermission { UserId = 1, PermissionId = 2 },
                new UserPermission { UserId = 1, PermissionId = 3 },
                new UserPermission { UserId = 1, PermissionId = 4 },
                new UserPermission { UserId = 1, PermissionId = 5 },
                new UserPermission { UserId = 1, PermissionId = 6 }
            );

            // Add other users with specific permissions
            var viewerUser = new User
            {
                Id = 2,
                UserName = "viewer",
                Email = "viewer@example.com",
                PasswordHash = "password123" // Use the static hash here
            };
            modelBuilder.Entity<User>().HasData(viewerUser);
            modelBuilder.Entity<UserPermission>().HasData(
                new UserPermission { UserId = 2, PermissionId = 1 }
            );
        }
    }
}