using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Dressings;

namespace ArtOfDesign.Application.Interfaces.Dressings;

public interface IDressingRepository
{
    Task<List<Dressing>> GetAllAsync();

    Task<Dressing?> GetByIdAsync(string id);

    Task AddAsync(Dressing dressing);

    Task UpdateAsync(Dressing dressing);

    Task DeleteImageAsync(ImageDressing image);
}
