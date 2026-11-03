
using ArtOfDesign.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArtOfDesign.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IImageStorageService _imageStorageService;

    public ImagesController(IImageStorageService imageStorageService)
    {
        _imageStorageService = imageStorageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Veuillez sélectionner une image.");

        await using var stream = file.OpenReadStream();

        var imageUrl = await _imageStorageService.UploadImageAsync(
            stream,
            file.FileName);

        return Ok(new { imageUrl });
    }
}
