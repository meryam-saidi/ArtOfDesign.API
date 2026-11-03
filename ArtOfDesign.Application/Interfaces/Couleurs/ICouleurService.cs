using ArtOfDesign.Application.DTOs.Couleurs;

namespace ArtOfDesign.Application.Interfaces.Couleurs;

public interface ICouleurService
{
    Task<List<CouleurDto>> GetAllAsync();

    Task<CouleurDto?> GetByIdAsync(int id);

    Task<CouleurDto> CreateAsync(
        CreateCouleurDto dto, 
        Stream imageStream, 
        string fileName);

    Task<CouleurDto?> UpdateAsync(
        int id, 
        UpdateCouleurDto dto,
        Stream? imageStream,
        string? fileName);

    Task<bool> DeleteAsync(int id);
}