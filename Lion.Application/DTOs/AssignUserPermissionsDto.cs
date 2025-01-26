using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Application.DTOs;
public class AssignUserPermissionsDto
{
    public string UserId { get; set; }
    public List<string> PermissionNames { get; set; }
}