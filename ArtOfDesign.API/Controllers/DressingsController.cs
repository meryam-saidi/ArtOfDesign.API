using ArtOfDesign.API.Extensions.Dressings;
using ArtOfDesign.Application.DTOs.Cuisines;
using ArtOfDesign.Application.DTOs.Dressings;
using ArtOfDesign.Application.DTOs.Dressings.Create;
using ArtOfDesign.Application.DTOs.Dressings.Update;
using ArtOfDesign.Application.Interfaces.Dressings;
using ArtOfDesign.Application.Services.Dressings;
using Microsoft.AspNetCore.Mvc;

namespace ArtOfDesign.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DressingsController : ControllerBase
{
    private readonly IDressingService _dressingService;

    public DressingsController(IDressingService DressingService)
    {
        _dressingService = DressingService;
    }

    [HttpGet]
    public async Task<ActionResult<List<DressingDto>>> GetAll()
    {
        var Dressings = await _dressingService.GetAllAsync();
        return Ok(Dressings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DressingDto>> GetById(string id)
    {
        var Dressing = await _dressingService.GetByIdAsync(id);

        if (Dressing == null) return NotFound();

        return Ok(Dressing);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateDressingDto dto,
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

                return new CreateDressingImageDto
                {
                    FileStream = stream,
                    FileName = Path.GetFileName(file.FileName),
                    OrdreAffichage = index + 1,
                    EstPrincipale = index == 0
                };
            }).ToList();

            var cuisine = await _dressingService.CreateAsync(dto);

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
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<DressingDto>> Update(
        string id,
        [FromForm] UpdateDressingForm form)
    {
        var dto = new UpdateDressingDto
        {
            Categorie = form.Categorie,
            Forme = form.Forme,
            TypeOuverture = form.TypeOuverture,
            Nom = form.Nom,
            PrixInitial = form.PrixInitial,
            Reduction = form.Reduction,
            Description = form.Description,

            Dimensions = new()
            {
                Largeur = form.Largeur,
                Hauteur = form.Hauteur,
                Profondeur = form.Profondeur
            },

            CaracteristiquesTechniques = new()
            {
                CompositionType = form.CompositionType,
                Caisson = form.Caisson,
                Facade = form.Facade
            },

            CouleurIds = form.CouleurIds
        };

        // Conversion des fichiers en streams
        var streams = new List<Stream>();

        try
        {
            foreach (var file in form.Images)
            {
                var stream = file.OpenReadStream();
                streams.Add(stream);

                // Remplacer DressingImageDto par le type réel
                // utilisé dans UpdateDressingDto.Images.
                dto.Images.Add(new UpdateImageDressingDto
                {
                    FileStream = stream,
                    FileName = file.FileName,
                    OrdreAffichage = streams.Count - 1,
                    EstPrincipale = streams.Count == 1
                });
            }

            var result = await _dressingService.UpdateAsync(id, dto);

            if (result == null)
                return NotFound(new { message = "Dressing introuvable." });

            return Ok(result);
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
        var deleted = await _dressingService.DeleteImageAsync(id, imageId);

        if (!deleted)
            return NotFound(new
            {
                message = "Cuisine ou image introuvable."
            });

        return NoContent();
    }
}
