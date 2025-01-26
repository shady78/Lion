using Lion.Application.Common.Interfaces;
using Lion.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lion.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserPermissionsController : ControllerBase
{
    private readonly IUserPermissionService _userPermissionService;
    private readonly ILogger<UserPermissionsController> _logger;

    public UserPermissionsController(
        IUserPermissionService userPermissionService,
        ILogger<UserPermissionsController> logger)
    {
        _userPermissionService = userPermissionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserPermissionDto>>> GetAllUserPermissions()
    {
        _logger.LogInformation("Getting all user permissions");
        var userPermissions = await _userPermissionService.GetAllUserPermissionsAsync();
        return Ok(userPermissions);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserPermissionDto>> GetUserPermissions(string userId)
    {
        _logger.LogInformation("Getting permissions for user: {UserId}", userId);
        var userPermissions = await _userPermissionService.GetUserPermissionsAsync(userId);
        return Ok(userPermissions);
    }

    [HttpGet("{userId}/effective")]
    public async Task<ActionResult<List<string>>> GetEffectivePermissions(string userId)
    {
        _logger.LogInformation("Getting effective permissions for user: {UserId}", userId);
        var permissions = await _userPermissionService.GetPermissionsForUserAsync(userId);
        return Ok(permissions);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignPermissions(AssignUserPermissionsDto assignPermissionsDto)
    {
        _logger.LogInformation("Assigning permissions to user: {UserId}", assignPermissionsDto.UserId);
        await _userPermissionService.AssignPermissionsToUserAsync(assignPermissionsDto);
        return NoContent();
    }
}
