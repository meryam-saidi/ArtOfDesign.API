using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Meubles;

namespace ArtOfDesign.Application.Interfaces.Meubles;

public interface IMeubleRepository
{
    Task<List<Meuble>> GetAllAsync();

    Task<Meuble?> GetByIdAsync(string id);

    Task AddAsync(Meuble meuble);

    Task UpdateAsync(Meuble meuble);

    Task DeleteImageAsync(ImageMeuble image);
}
