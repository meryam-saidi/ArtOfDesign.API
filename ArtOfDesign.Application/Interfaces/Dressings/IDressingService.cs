using ArtOfDesign.Application.DTOs.Dressings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.Interfaces.Dressings;

public interface IDressingService
{
    Task<List<DressingDto>> GetAllAsync();

    Task<DressingDto?> GetByIdAsync(string id);

    Task<DressingDto> CreateAsync(CreateDressingDto dto);

    Task<DressingDto?> UpdateAsync(string id, UpdateDressingDto dto);

   /* Task<DressingDto?> PatchAsync(
        string id,
        PatchDressingDto dto,
        List<CreateDressingImageDto> nouvellesImages);*/

    Task<bool> DeleteImageAsync(string DressingId, int imageId);
}
