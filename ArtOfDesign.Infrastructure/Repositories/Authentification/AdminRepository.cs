using ArtOfDesign.Application.Interfaces.Authentification;
using ArtOfDesign.Domain.Entities.Authentification;
using ArtOfDesign.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace ArtOfDesign.Infrastructure.Repositories.Authentification;

public class AdminRepository : IAdminRepository
{
    private readonly AppDbContext _context;
    public AdminRepository(AppDbContext context) => _context = context;

    public Task<Admin?> GetByEmailAsync(string email)
        => _context.Admins.FirstOrDefaultAsync(a => a.Email == email);

    public Task<Admin?> GetByIdAsync(int id)
        => _context.Admins.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

    public Task UpdateAsync(Admin admin) => _context.SaveChangesAsync();
}
