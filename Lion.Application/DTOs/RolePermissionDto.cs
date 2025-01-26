using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Application.DTOs;
public class RolePermissionDto
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public List<string> PermissionNames { get; set; }
}