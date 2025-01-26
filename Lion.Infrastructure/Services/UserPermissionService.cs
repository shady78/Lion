using Lion.Application.Common.Interfaces;
using Lion.Application.DTOs;
using Lion.Domain.Entities;
using Lion.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Infrastructure.Services;
public class UserPermissionService : IUserPermissionService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserPermissionService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<UserPermissionDto>> GetAllUserPermissionsAsync()
    {
        var userPermissions = await _context.Users
            .Include(u => u.UserPermissions)
                .ThenInclude(up => up.Permission)
            .Include(u => u.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .Select(u => new UserPermissionDto
            {
                UserId = u.Id,
                UserEmail = u.Email,
                PermissionNames = u.UserPermissions
                    .Select(up => up.Permission.Name)
                    .Union(u.RolePermissions
                        .Select(rp => rp.Permission.Name))
                    .ToList()
            })
            .ToListAsync();

        return userPermissions;
    }

    public async Task<UserPermissionDto> GetUserPermissionsAsync(string userId)
    {
        var user = await _context.Users
            .Include(u => u.UserPermissions)
            .ThenInclude(up => up.Permission)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new Exception();
        }

        return new UserPermissionDto
        {
            UserId = user.Id,
            UserEmail = user.Email,
            PermissionNames = user.UserPermissions
                .Select(up => up.Permission.Name)
                .ToList()
        };
    }

    public async Task AssignPermissionsToUserAsync(AssignUserPermissionsDto assignPermissionsDto)
    {
        var user = await _context.Users
            .Include(u => u.UserPermissions)
            .FirstOrDefaultAsync(u => u.Id == assignPermissionsDto.UserId);

        if (user == null)
        {
            return;
        }

        // Get all permissions that need to be assigned
        var permissions = await _context.Permissions
            .Where(p => assignPermissionsDto.PermissionNames.Contains(p.Name))
            .ToListAsync();

        // Validate all permissions exist
        if (permissions.Count != assignPermissionsDto.PermissionNames.Count)
        {
            var existingPermissionNames = permissions.Select(p => p.Name);
            var invalidPermissions = assignPermissionsDto.PermissionNames
                .Except(existingPermissionNames);

            throw new ValidationException($"Invalid permissions: {string.Join(", ", invalidPermissions)}");
        }

        // Remove existing permissions
        _context.UserPermissions.RemoveRange(user.UserPermissions);

        // Assign new permissions
        foreach (var permission in permissions)
        {
            user.UserPermissions.Add(new UserPermission
            {
                UserId = user.Id,
                PermissionId = permission.Id
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<string>> GetPermissionsForUserAsync(string userId)
    {
        // Get direct user permissions
        var directPermissions = await _context.UserPermissions
            .Include(up => up.Permission)
            .Where(up => up.UserId == userId)
            .Select(up => up.Permission.Name)
            .ToListAsync();

        // Get role-based permissions
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var rolePermissions = await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => userRoles.Contains(rp.RoleId))
                .Select(rp => rp.Permission!.Name)
                .ToListAsync();

            // Combine and deduplicate permissions
            directPermissions.AddRange(rolePermissions);
            directPermissions = directPermissions.Distinct().ToList();
        }

        return directPermissions;
    }
}