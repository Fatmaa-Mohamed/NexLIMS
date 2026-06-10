using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using NexLIMS.API.Middlewares;
    using System.Security.Claims;
using NextLIMS.DAL.Data;

namespace NexLIMS.API.Controllers.middlewares
{
    public class PermissionBasedAuthFilter : IAuthorizationFilter
    {
        private readonly ApplicationDbContext _context;

        public PermissionBasedAuthFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permissionString = context.ActionDescriptor.EndpointMetadata
                .OfType<CheckPermissionAttribute>()
                .FirstOrDefault()?.Permission;

            if (permissionString == null) return;

            var user = context.HttpContext.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var tenantIdClaim = user.FindFirst("TenantId")?.Value;
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (tenantIdClaim == null || userIdClaim == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var tenantId = int.Parse(tenantIdClaim);
            var userId = int.Parse(userIdClaim);

            var dbUser = _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefault(u => u.Id == userId && u.TenantId == tenantId);

            if (dbUser == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userPermissions = dbUser.Role?.RolePermissions
                .Select(rp => rp.Permission.Name)
                .ToList();

            if (userPermissions == null || !userPermissions.Contains(permissionString))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}