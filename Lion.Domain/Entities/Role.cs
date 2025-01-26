using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Domain.Entities;
public class Role : IdentityRole
{
    public Role() : base()
    {
        RolePermissions = new HashSet<RolePermission>();
    }

    public Role(string roleName) : base(roleName)
    {
        RolePermissions = new HashSet<RolePermission>();
    }

    public virtual ICollection<RolePermission> RolePermissions { get; set; }
}