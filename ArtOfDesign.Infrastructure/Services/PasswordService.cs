using ArtOfDesign.Application.Interfaces.Authentification;
using ArtOfDesign.Domain.Entities.Authentification;
using Microsoft.AspNetCore.Identity;


namespace ArtOfDesign.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<Admin> _hasher = new();

    public string Hash(Admin admin, string password)
        => _hasher.HashPassword(admin, password);

    public bool Verify(Admin admin, string password, out bool needsRehash)
    {
        var result = _hasher.VerifyHashedPassword(admin, admin.PasswordHash, password);
        needsRehash = result == PasswordVerificationResult.SuccessRehashNeeded;
        return result != PasswordVerificationResult.Failed;
    }
}
