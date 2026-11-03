using ArtOfDesign.Application.Interfaces.Couleurs;
using ArtOfDesign.Domain.Entities.Catalogue;
using ArtOfDesign.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtOfDesign.Infrastructure.Repositories.Couleurs;

public class CouleurRepository : ICouleurRepository
{
    private readonly AppDbContext _context;

    public CouleurRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Couleur>> GetAllAsync()
    {
        return await _context.Couleurs
            .AsNoTracking()
            .OrderBy(c => c.Nom)
            .ToListAsync();
    }

    public async Task<Couleur?> GetByIdAsync(int id)
    {
        return await _context.Couleurs
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Couleur couleur)
    {
        await _context.Couleurs.AddAsync(couleur);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Couleur couleur)
    {
        _context.Couleurs.Update(couleur);
        await _context.SaveChangesAsync();
    }
}