using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Domain.Entities;
public class UserPermission
{
    public string UserId { get; set; }
    public int PermissionId { get; set; }
    public virtual ApplicationUser User { get; set; }
    public virtual Permission Permission { get; set; }
}