using ArtOfDesign.Application.Interfaces.Dressings;
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Dressings;
using ArtOfDesign.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Infrastructure.Repositories.Dressings;

public class DressingRepository : IDressingRepository
{
    private readonly AppDbContext _appDbContext;

    public DressingRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Dressing>> GetAllAsync()
    {
        return await _appDbContext.Dressings
            .Include(d => d.Dimensions)
            .Include(d => d.Images)
            .Include(d => d.CaracteristiquesTechniques)
            .Include(d => d.DressingCouleurs)
                .ThenInclude(dc => dc.Couleur)
            .ToListAsync();
    }

    public async Task<Dressing?> GetByIdAsync(string id)
    {
        return await _appDbContext.Dressings
            .Include (d => d.Dimensions)
            .Include (d => d.Images)
            .Include (d => d.CaracteristiquesTechniques)
            .Include (d => d.DressingCouleurs)
            .ThenInclude (dc => dc.Couleur)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Dressing dressing)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        await _appDbContext.Dressings.AddAsync(dressing, cts.Token);
        await _appDbContext.SaveChangesAsync(cts.Token);
    }

    public async Task UpdateAsync(Dressing dressing)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        // Une autre façon de faire les choses
        // Entity Framework suit déjà l'entité si elle a été récupérée dans le même contexte,
        // mais on peut explicitement marquer l'état ou sauvegarder.
        _appDbContext.Dressings.Update(dressing);
        await _appDbContext.SaveChangesAsync(cts.Token);
    }

    public async Task DeleteImageAsync(ImageDressing image)
    {
        _appDbContext.ImagesDressing.Remove(image);
        await _appDbContext.SaveChangesAsync();
    }
}
