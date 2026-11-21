using ArtOfDesign.Application.Interfaces.Meubles;
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Meubles;
using ArtOfDesign.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace ArtOfDesign.Infrastructure.Repositories.Meubles;

public class MeubleRepository : IMeubleRepository
{
    private readonly AppDbContext _appDbContext;

    public MeubleRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Meuble>> GetAllAsync()
    {
        return await _appDbContext.Meubles
            .Include(d => d.Dimensions)
            .Include(d => d.Images)
            .Include(d => d.CaracteristiquesTechniques)
            .Include(d => d.MeubleCouleurs)
                .ThenInclude(dc => dc.Couleur)
            .ToListAsync();
    }

    public async Task<Meuble?> GetByIdAsync(string id)
    {
        return await _appDbContext.Meubles
            .Include(d => d.Dimensions)
            .Include(d => d.Images)
            .Include(d => d.CaracteristiquesTechniques)
            .Include(d => d.MeubleCouleurs)
            .ThenInclude(dc => dc.Couleur)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Meuble meuble)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        await _appDbContext.Meubles.AddAsync(meuble, cts.Token);
        await _appDbContext.SaveChangesAsync(cts.Token);
    }

    public async Task UpdateAsync(Meuble meuble)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        _appDbContext.Meubles.Update(meuble);
        await _appDbContext.SaveChangesAsync(cts.Token);
    }

    public async Task DeleteImageAsync(ImageMeuble image)
    {
        _appDbContext.ImagesMeuble.Remove(image);
        await _appDbContext.SaveChangesAsync();
    }
}
