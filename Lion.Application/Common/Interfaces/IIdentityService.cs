using Lion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Application.Common.Interfaces;
public interface IIdentityService
{
    Task<(bool Success, string UserId)> CreateUserAsync(RegisterDto registerDto);
    Task<(bool Success, string Token)> LoginAsync(LoginDto loginDto);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> AuthorizeAsync(string userId, string policyName);
}