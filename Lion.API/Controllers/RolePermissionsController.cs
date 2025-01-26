using Lion.Application.Common.Interfaces;
using Lion.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lion.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RolePermissionsController : ControllerBase
{
    private readonly IRolePermissionService _rolePermissionService;
    private readonly ILogger<RolePermissionsController> _logger;

    public RolePermissionsController(
        IRolePermissionService rolePermissionService,
        ILogger<RolePermissionsController> logger)
    {
        _rolePermissionService = rolePermissionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<RolePermissionDto>>> GetAllRolePermissions()
    {
        _logger.LogInformation("Getting all role permissions");
        var rolePermissions = await _rolePermissionService.GetAllRolePermissionsAsync();
        return Ok(rolePermissions);
    }

    [HttpGet("{roleId}")]
    public async Task<ActionResult<RolePermissionDto>> GetRolePermissions(string roleId)
    {
        _logger.LogInformation("Getting permissions for role: {RoleId}", roleId);
        var rolePermissions = await _rolePermissionService.GetRolePermissionsAsync(roleId);
        return Ok(rolePermissions);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignPermissions(AssignPermissionsDto assignPermissionsDto)
    {
        _logger.LogInformation("Assigning permissions to role: {RoleId}", assignPermissionsDto.RoleId);
        await _rolePermissionService.AssignPermissionsToRoleAsync(assignPermissionsDto);
        return NoContent();
    }
}