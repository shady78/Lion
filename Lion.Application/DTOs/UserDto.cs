using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Application.DTOs;
public class UserDto
{
    public string Id { get; set; } = null!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}