using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ArtOfDesign.Application.Interfaces;
using ArtOfDesign.Application.DTOs.Images;
using AppImageUploadResult =
    ArtOfDesign.Application.DTOs.Images.ImageUploadResult;

namespace ArtOfDesign.Infrastructure.Services; 

public class CloudinaryService : IImageStorageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(string cloudName, string apiKey, string apiSecret)
    {
        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<AppImageUploadResult> UploadImageAsync(
        Stream fileStream,
        string fileName)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = "art-of-design"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
            throw new InvalidOperationException(result.Error.Message);

        return new AppImageUploadResult
        {
            Url = result.SecureUrl?.ToString()
                ?? throw new InvalidOperationException(
                    "Cloudinary n'a pas retourné d'URL."),
            PublicId = result.PublicId
        };
    }

    public async Task DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Error != null)
            throw new InvalidOperationException(result.Error.Message);

        if (result.Result != "ok" && result.Result != "not found")
            throw new InvalidOperationException(
                $"La suppression Cloudinary a échoué : {result.Result}");
    }

}
