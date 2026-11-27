using ArtOfDesign.API.Extensions.Meubles;
using ArtOfDesign.Application.DTOs.Meubles;
using ArtOfDesign.Application.DTOs.Meubles.Create;
using ArtOfDesign.Application.DTOs.Meubles.Update;
using ArtOfDesign.Application.Interfaces.Meubles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtOfDesign.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeublesController : ControllerBase
{
    private readonly IMeubleService _meubleService;

    public MeublesController(IMeubleService MeubleService)
    {
        _meubleService = MeubleService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<MeubleDto>>> GetAll()
    {
        var Meubles = await _meubleService.GetAllAsync();
        return Ok(Meubles);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<MeubleDto>> GetById(string id)
    {
        var Meuble = await _meubleService.GetByIdAsync(id);

        if (Meuble == null) return NotFound();

        return Ok(Meuble);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateMeubleDto dto,
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

                return new CreateMeubleImageDto
                {
                    FileStream = stream,
                    FileName = Path.GetFileName(file.FileName),
                    OrdreAffichage = index + 1,
                    EstPrincipale = index == 0
                };
            }).ToList();

            var meuble = await _meubleService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = meuble.Id },
                meuble);
        }
        finally
        {
            foreach (var stream in streams)
                await stream.DisposeAsync();
        }
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<MeubleDto>> Update(
        string id,
        [FromForm] UpdateMeubleForm form)
    {
        var dto = new UpdateMeubleDto
        {
            //Categorie = form.Categorie,
            Forme = form.Forme,
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

                dto.Images.Add(new UpdateMeubleImageDto
                {
                    FileStream = stream,
                    FileName = file.FileName,
                    OrdreAffichage = streams.Count - 1,
                    EstPrincipale = streams.Count == 1
                });
            }

            var result = await _meubleService.UpdateAsync(id, dto);

            if (result == null)
                return NotFound(new { message = "Meuble introuvable." });

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
        var deleted = await _meubleService.DeleteImageAsync(id, imageId);

        if (!deleted)
            return NotFound(new
            {
                message = "Meuble ou image introuvable."
            });

        return NoContent();
    }
}
