
using ArtOfDesign.Application.Interfaces.Cuisines;
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Catalogue;
using ArtOfDesign.Domain.Entities.Cuisines;
using ArtOfDesign.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtOfDesign.Infrastructure.Repositories.Cuisines;

public class CuisineRepository : ICuisineRepository
{
    private readonly AppDbContext _context;

    public CuisineRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cuisine>> GetAllAsync()
    {
        return await _context.Cuisines
            .Include(c => c.Dimensions)
            .Include(c => c.Images)
            .Include(c => c.CaracteristiquesTechniques)
            .Include(c => c.CuisineCouleurs)
                .ThenInclude(cc => cc.Couleur)
            .ToListAsync();
    }

    public async Task<Cuisine?> GetByIdAsync(string id)
    {
        return await _context.Cuisines
            .Include(c => c.Dimensions)
            .Include(c => c.Images)
            .Include(c => c.CaracteristiquesTechniques)
            .Include(c => c.CuisineCouleurs)
                .ThenInclude(cc => cc.Couleur)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Cuisine cuisine)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        await _context.Cuisines.AddAsync(cuisine, cts.Token);
        await _context.SaveChangesAsync(cts.Token);
    }

    public async Task UpdateAsync(Cuisine cuisine)
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteImageAsync(ImageCuisine image)
    {
        _context.ImagesCuisine.Remove(image);
        await _context.SaveChangesAsync();
    } 
}
