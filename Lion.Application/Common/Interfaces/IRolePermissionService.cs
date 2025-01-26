using Lion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Application.Common.Interfaces;
public interface IRolePermissionService
{
    Task<List<RolePermissionDto>> GetAllRolePermissionsAsync();
    Task<RolePermissionDto> GetRolePermissionsAsync(string roleId);
    Task AssignPermissionsToRoleAsync(AssignPermissionsDto assignPermissionsDto);
    Task<List<string>> GetPermissionsForRoleAsync(string roleId);
}