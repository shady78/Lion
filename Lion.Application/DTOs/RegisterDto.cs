using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Application.DTOs;
public class RegisterDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
}