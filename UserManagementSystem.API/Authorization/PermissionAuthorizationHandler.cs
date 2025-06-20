using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UserManagementSystem.Application.IServices;

namespace UserManagementSystem.API.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<PermissionAuthorizationHandler> _logger;

        public PermissionAuthorizationHandler(IPermissionService permissionService, ILogger<PermissionAuthorizationHandler> logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogWarning("Permission check failed: User ID claim not found or invalid.");
                context.Fail();
                return;
            }

            bool hasPermission = await _permissionService.HasPermissionAsync(userId, requirement.PermissionName);

            if (hasPermission)
            {
                _logger.LogInformation($"User {userId} has permission '{requirement.PermissionName}'. Authorization successful.");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogWarning($"User {userId} does NOT have permission '{requirement.PermissionName}'. Authorization failed.");
                context.Fail();
            }
        }
    }
}
