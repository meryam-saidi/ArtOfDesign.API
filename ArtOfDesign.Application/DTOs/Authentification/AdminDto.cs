using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Authentification;

public class AdminDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? UserName { get; set; }
}
