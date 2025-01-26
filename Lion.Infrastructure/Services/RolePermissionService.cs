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
public class RolePermissionService : IRolePermissionService
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<Role> _roleManager;

    public RolePermissionService(
        ApplicationDbContext context,
        RoleManager<Role> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    public async Task<List<RolePermissionDto>> GetAllRolePermissionsAsync()
    {
        var rolePermissions = await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Select(r => new RolePermissionDto
            {
                RoleId = r.Id,
                RoleName = r.Name!,
                PermissionNames = r.RolePermissions
                    .Select(rp => rp.Permission!.Name!)
                    .ToList()
            })
            .ToListAsync();

        return rolePermissions;
    }

    public async Task<RolePermissionDto> GetRolePermissionsAsync(string roleId)
    {
        var role = await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == roleId);

        if (role == null)
        {
            throw new Exception();
        }

        return new RolePermissionDto
        {
            RoleId = role.Id,
            RoleName = role.Name!,
            PermissionNames = role.RolePermissions!
                .Select(rp => rp.Permission!.Name!)
                .ToList()
        };
    }

    public async Task AssignPermissionsToRoleAsync(AssignPermissionsDto assignPermissionsDto)
    {
        var role = await _context.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == assignPermissionsDto.RoleId);

        if (role == null)
        {
            throw new Exception();
        }

        // Get all permissions that need to be assigned
        var permissions = await _context.Permissions
            .Where(p => assignPermissionsDto.PermissionNames.Contains(p.Name!))
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
        _context.RolePermissions.RemoveRange(role.RolePermissions);

        // Assign new permissions
        foreach (var permission in permissions)
        {
            role.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id,
                PermissionId = permission.Id
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<string>> GetPermissionsForRoleAsync(string roleId)
    {
        var permissions = await _context.RolePermissions
            .Include(rp => rp.Permission)
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.Permission!.Name)
            .ToListAsync();

        return permissions!;
    }
}