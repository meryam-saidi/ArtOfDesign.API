using ArtOfDesign.Application.DTOs.Cuisines;
using ArtOfDesign.Application.DTOs.Cuisines.Patch;

namespace ArtOfDesign.Application.Interfaces.Cuisines;

public interface ICuisineService
{
    Task<List<CuisineDto>> GetAllAsync();

    Task<CuisineDto?> GetByIdAsync(string id);

    Task<CuisineDto> CreateAsync(CreateCuisineDto dto);

    Task<CuisineDto?> UpdateAsync(string id, UpdateCuisineDto dto);

    Task<CuisineDto?> PatchAsync(
        string id,
        PatchCuisineDto dto,
        List<CreateCuisineImageDto> nouvellesImages);

    Task<bool> DeleteImageAsync(string cuisineId, int imageId);
}