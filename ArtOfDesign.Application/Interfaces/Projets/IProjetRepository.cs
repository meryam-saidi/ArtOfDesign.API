using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Projets;


namespace ArtOfDesign.Application.Interfaces.Projets;

public interface IProjetRepository : IExistsById
{
    Task<List<Projet>> GetAllAsync();

    Task<Projet?> GetByIdAsync(string id);

    Task AddAsync(Projet projet);

    Task UpdateAsync(Projet projet);

    Task DeleteImageAsync(ImageProjet image);

    Task DeleteAsync(Projet projet);
}
