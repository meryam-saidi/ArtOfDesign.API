
using ArtOfDesign.Application.DTOs.Cuisines;
using ArtOfDesign.Application.DTOs.Cuisines.Patch;
using ArtOfDesign.Application.Interfaces.Cuisines;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ArtOfDesign.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CuisinesController : ControllerBase
{
    private readonly ICuisineService _cuisineService;

    public CuisinesController(ICuisineService cuisineService)
    {
        _cuisineService = cuisineService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CuisineDto>>> GetAll()
    {
        var cuisines = await _cuisineService.GetAllAsync();
        return Ok(cuisines);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CuisineDto>> GetById(string id)
    {
        var couleur = await _cuisineService.GetByIdAsync(id);

        if (couleur is null)
            return NotFound();

        return Ok(couleur);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateCuisineDto dto,
        [FromForm] List<IFormFile> files)
    {
        if (files is null || files.Count == 0)
            return BadRequest(new { message = "Ajoute au moins une image." });

        if (files.Any(f => f.Length == 0))
            return BadRequest(new { message = "Un des fichiers est vide." });

        var streams = new List<Stream>();

        try
        {
            dto.Images = files.Select((file, index) =>
            {
                var stream = file.OpenReadStream();
                streams.Add(stream);

                return new CreateCuisineImageDto
                {
                    FileStream = stream,
                    FileName = Path.GetFileName(file.FileName),
                    OrdreAffichage = index + 1,
                    EstPrincipale = index == 0
                };
            }).ToList();

            var cuisine = await _cuisineService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = cuisine.Id },
                cuisine);
        }
        finally
        {
            foreach (var stream in streams)
                await stream.DisposeAsync();
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
    string id,
    [FromBody] UpdateCuisineDto dto)
    {
        try
        {
            var cuisine = await _cuisineService.UpdateAsync(id, dto);

            if (cuisine is null)
                return NotFound(new
                {
                    message = $"Aucune cuisine trouvée avec l'id '{id}'."
                });

            return Ok(cuisine);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpPatch("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Patch(
        string id,
        [FromForm] string? dto,
        [FromForm] List<IFormFile>? files)
    {
        PatchCuisineDto? patchDto = null;

        if (!string.IsNullOrWhiteSpace(dto))
        {
            try
            {
                patchDto = JsonSerializer.Deserialize<PatchCuisineDto>(
                    dto,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (JsonException)
            {
                return BadRequest(new
                {
                    message = "Le JSON fourni est invalide."
                });
            }

            if (patchDto is null)
            {
                return BadRequest(new
                {
                    message = "Le JSON fourni est invalide."
                });
            }
        }

        var streams = new List<Stream>();

        try
        {
            var nouvellesImages = new List<CreateCuisineImageDto>();

            foreach (var file in files ?? new List<IFormFile>())
            {
                if (file.Length == 0)
                    continue;

                var stream = file.OpenReadStream();
                streams.Add(stream);

                nouvellesImages.Add(new CreateCuisineImageDto
                {
                    FileStream = stream,
                    FileName = Path.GetFileName(file.FileName)
                });
            }

            if (patchDto is null && nouvellesImages.Count == 0)
            {
                return BadRequest(new
                {
                    message = "Aucune modification fournie."
                });
            }
            
            patchDto ??= new PatchCuisineDto();

            var cuisine = await _cuisineService.PatchAsync(
                id,
                patchDto,
                nouvellesImages);

            if (cuisine is null)
            {
                return NotFound(new
                {
                    message = $"La cuisine avec l'identifiant {id} est introuvable."
                });
            }

            return Ok(cuisine);
        }
        finally
        {
            foreach (var stream in streams)
                await stream.DisposeAsync();
        }
    }

    [HttpDelete("{id}/images/{imageId:int}")]
    public async Task<IActionResult> DeleteImage(string id, int imageId)
    {
        var deleted = await _cuisineService.DeleteImageAsync(id, imageId);

        if (!deleted)
            return NotFound(new
            {
                message = "Cuisine ou image introuvable."
            });

        return NoContent();
    }
}
