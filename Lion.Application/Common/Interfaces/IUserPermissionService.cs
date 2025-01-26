using Lion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Application.Common.Interfaces;
public interface IUserPermissionService
{
    Task<List<UserPermissionDto>> GetAllUserPermissionsAsync();
    Task<UserPermissionDto> GetUserPermissionsAsync(string userId);
    Task AssignPermissionsToUserAsync(AssignUserPermissionsDto assignPermissionsDto);
    Task<List<string>> GetPermissionsForUserAsync(string userId);
}
