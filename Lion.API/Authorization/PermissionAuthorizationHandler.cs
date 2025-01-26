using Lion.Domain.Entities;
using Lion.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lion.API.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public PermissionAuthorizationHandler(
        ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var user = await _userManager.GetUserAsync(context.User);
        if (user == null)
        {
            return;
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var hasPermission = await _dbContext.RolePermissions
            .Include(rp => rp.Permission)
            .AnyAsync(rp =>
                userRoles.Contains(rp.RoleId) &&
                rp.Permission!.Name == requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}