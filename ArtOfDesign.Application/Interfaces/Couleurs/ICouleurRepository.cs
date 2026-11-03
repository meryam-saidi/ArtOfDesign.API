using ArtOfDesign.Domain.Entities.Catalogue;

namespace ArtOfDesign.Application.Interfaces.Couleurs;

public interface ICouleurRepository
{
    Task<List<Couleur>> GetAllAsync();

    Task<Couleur?> GetByIdAsync(int id);

    Task AddAsync(Couleur couleur);

    Task UpdateAsync(Couleur couleur);
}