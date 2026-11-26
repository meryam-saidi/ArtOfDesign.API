using ArtOfDesign.Application.Interfaces.Projets;
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Projets;
using ArtOfDesign.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtOfDesign.Infrastructure.Repositories.Projets;

public class ProjetRepository : IProjetRepository
{
    private readonly AppDbContext _appDbContext;

    public ProjetRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task<bool> ExistsAsync(string id)
    => _appDbContext.Projets.AnyAsync(p => p.Id == id);

    public async Task<List<Projet>> GetAllAsync()
    {
        return await _appDbContext.Projets
            .Include(d => d.Images)
            .ToListAsync();
    }

    public async Task<Projet?> GetByIdAsync(string id)
    {
        return await _appDbContext.Projets
            .Include(d => d.Images)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Projet projet)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        await _appDbContext.Projets.AddAsync(projet, cts.Token);
        await _appDbContext.SaveChangesAsync(cts.Token);
    }

    public async Task UpdateAsync(Projet projet)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        _appDbContext.Projets.Update(projet);
        await _appDbContext.SaveChangesAsync(cts.Token);
    }

    public async Task DeleteImageAsync(ImageProjet image)
    {
        _appDbContext.ImagesProjet.Remove(image);
        await _appDbContext.SaveChangesAsync();
    }

    public Task DeleteAsync(Projet projet)
    {
        throw new NotImplementedException();
    }
}
