
using ArtOfDesign.Application.DTOs.Meubles;
using ArtOfDesign.Application.DTOs.Meubles.Create;
using ArtOfDesign.Application.DTOs.Meubles.Update;

namespace ArtOfDesign.Application.Interfaces.Meubles;

public interface IMeubleService
{
    Task<List<MeubleDto>> GetAllAsync();

    Task<MeubleDto?> GetByIdAsync(string id);

    Task<MeubleDto> CreateAsync(CreateMeubleDto dto);

    Task<MeubleDto?> UpdateAsync(string id, UpdateMeubleDto dto);

    Task<bool> DeleteImageAsync(string meubleId, int imageId);
}
