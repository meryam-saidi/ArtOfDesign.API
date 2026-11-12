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

    public async Task AddAsync(Dressing cuisine)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        await _appDbContext.Dressings.AddAsync(cuisine, cts.Token);
        await _appDbContext.SaveChangesAsync(cts.Token);
    }

    public Task DeleteImageAsync(ImageDressing image)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Dressing cuisine)
    {
        throw new NotImplementedException();
    }
}
