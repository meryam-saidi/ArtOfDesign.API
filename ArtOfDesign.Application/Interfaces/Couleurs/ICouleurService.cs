using ArtOfDesign.Application.DTOs.Couleurs;

namespace ArtOfDesign.Application.Interfaces.Couleurs;

public interface ICouleurService
{
    Task<List<CouleurDto>> GetAllAsync();

    Task<CouleurDto?> GetByIdAsync(int id);

    Task<CouleurDto> CreateAsync(CreateCouleurDto dto);

    Task<CouleurDto?> UpdateAsync(int id, CouleurDto dto);
}