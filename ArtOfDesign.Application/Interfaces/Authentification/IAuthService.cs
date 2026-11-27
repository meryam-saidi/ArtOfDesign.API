using ArtOfDesign.Application.DTOs.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.Interfaces.Authentification;

public interface IAuthService
{
    Task<AdminDto?> ValidateCredentialsAsync(string email, string password);
    Task<AdminDto?> GetAdminAsync(int id);
}
