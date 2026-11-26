
using ArtOfDesign.Application.DTOs.Projets;
using ArtOfDesign.Application.DTOs.Projets.Create;
using ArtOfDesign.Application.DTOs.Projets.Update;

namespace ArtOfDesign.Application.Interfaces.Projets;

public interface IProjetService
{
    Task<List<ProjetDto>> GetAllAsync();

    Task<ProjetDto?> GetByIdAsync(string id);

    Task<ProjetDto> CreateAsync(CreateProjetDto dto);

    Task<ProjetDto?> UpdateAsync(string id, UpdateProjetDto dto);

    Task<bool> DeleteImageAsync(string projetId, int imageId);

    Task<bool> DeleteAsync(string id);
}
