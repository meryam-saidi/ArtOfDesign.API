using ArtOfDesign.Domain.Entities.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.Interfaces.Authentification;

public interface IAdminRepository
{
    Task<Admin?> GetByEmailAsync(string email);
    Task<Admin?> GetByIdAsync(int id);
    Task UpdateAsync(Admin admin);
}
