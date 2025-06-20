using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace UserManagementSystem.API.Authorization
{
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
            {
                return policy;
            }

            if (policyName.StartsWith("Permission:", System.StringComparison.OrdinalIgnoreCase))
            {
                var permissionName = policyName.Substring("Permission:".Length);
                var builder = new AuthorizationPolicyBuilder();
                builder.AddRequirements(new PermissionRequirement(permissionName)); // أنشئ Requirement بالإذن الصحيح
                return builder.Build();
            }

            return null;
        }
    }
}
