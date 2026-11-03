using ArtOfDesign.Application.DTOs.Images;

namespace ArtOfDesign.Application.Interfaces;

public interface IImageStorageService
{
    Task<ImageUploadResult> UploadImageAsync(Stream fileStream, string fileName);
    Task DeleteImageAsync(string publicId);
}
