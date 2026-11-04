
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Cuisines;

namespace ArtOfDesign.Application.Interfaces.Cuisines;

public interface ICuisineRepository
{
    Task<List<Cuisine>> GetAllAsync();

    Task<Cuisine?> GetByIdAsync(string id);

    Task AddAsync(Cuisine cuisine);

    Task UpdateAsync(Cuisine cuisine);

    Task DeleteImageAsync(ImageCuisine image);
}
