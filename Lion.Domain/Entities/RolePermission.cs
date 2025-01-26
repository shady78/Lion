using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Domain.Entities;
public class RolePermission
{
    public string RoleId { get; set; }
    public int PermissionId { get; set; }
    public virtual Role? Role { get; set; }
    public virtual Permission? Permission { get; set; }
}