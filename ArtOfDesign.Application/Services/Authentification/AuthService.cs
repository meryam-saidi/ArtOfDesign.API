using ArtOfDesign.Application.DTOs.Authentification;
using ArtOfDesign.Application.Interfaces.Authentification;
using ArtOfDesign.Domain.Entities.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.Services.Authentification;

public class AuthService : IAuthService
{
    private readonly IAdminRepository _adminRepository;
    private readonly IPasswordService _passwordService;

    public AuthService(IAdminRepository adminRepository, IPasswordService passwordService)
    {
        _adminRepository = adminRepository;
        _passwordService = passwordService;
    }

    public async Task<AdminDto?> ValidateCredentialsAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return null;

        var admin = await _adminRepository.GetByEmailAsync(email.Trim().ToLowerInvariant());
        if (admin is null)
            return null;

        if (!_passwordService.Verify(admin, password, out var needsRehash))
            return null;

        // Hash créé avec un ancien algorithme → on le met à jour
        if (needsRehash)
            admin.PasswordHash = _passwordService.Hash(admin, password);

        admin.DerniereConnexion = DateTime.UtcNow;
        await _adminRepository.UpdateAsync(admin);

        return MapToDto(admin);
    }

    public async Task<AdminDto?> GetAdminAsync(int id)
    {
        var admin = await _adminRepository.GetByIdAsync(id);
        return admin is null ? null : MapToDto(admin);
    }

    private static AdminDto MapToDto(Admin a) => new()
    {
        Id = a.Id,
        Email = a.Email,
        UserName = a.UserName
    };
}
