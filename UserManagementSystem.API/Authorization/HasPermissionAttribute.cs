using Microsoft.AspNetCore.Authorization;
using System;

namespace UserManagementSystem.API.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permissionName)
        {
            Policy = $"Permission:{permissionName}";
        }
    }
}