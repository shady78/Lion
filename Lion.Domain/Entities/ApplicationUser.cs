using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        UserPermissions = new HashSet<UserPermission>();
    }
    public string? CompanyName { get; set; }
    public virtual ICollection<RolePermission>? RolePermissions { get; set; }
    public virtual ICollection<UserPermission> UserPermissions { get; set; }

}