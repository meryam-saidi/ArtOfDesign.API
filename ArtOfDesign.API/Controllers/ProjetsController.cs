using ArtOfDesign.API.Extensions.Projets;
using ArtOfDesign.Application.DTOs.Projets.Create;
using ArtOfDesign.Application.DTOs.Projets.Update;
using ArtOfDesign.Application.DTOs.Projets;
using ArtOfDesign.Application.Interfaces.Projets;
using Microsoft.AspNetCore.Mvc;

namespace ArtOfDesign.API.Controllers;

[ApiController]
[Route("api/projets")]
public class ProjetsController : ControllerBase
{
    private readonly IProjetService _projetService;

    private const long TailleMaxImage = 10 * 1024 * 1024; // 10 Mo
    private static readonly string[] TypesAutorises = { "image/jpeg", "image/png", "image/webp" };

    public ProjetsController(IProjetService projetService)
    {
        _projetService = projetService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjetDto>>> GetAll()
        => Ok(await _projetService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjetDto>> GetById(string id)
    {
        var projet = await _projetService.GetByIdAsync(id);
        return projet is null ? NotFound() : Ok(projet);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(200_000_000)]
    public async Task<ActionResult<ProjetDto>> Create([FromForm] ProjetCreateForm form)
    {
        var fichiers = form.Images3D.Select(f => (Fichier: f, Est3D: true))
            .Concat(form.ImagesReelles.Select(f => (Fichier: f, Est3D: false)))
            .OrderBy(x => x.Fichier.FileName, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var erreur = ValiderFichiers(fichiers.Select(x => x.Fichier));
        if (erreur is not null) return BadRequest(erreur);

        var streams = new List<Stream>();
        try
        {
            var dto = new CreateProjetDto
            {
                Nom = form.Nom,
                Description = form.Description,
                Images = fichiers.Select((x, i) =>
                {
                    var stream = x.Fichier.OpenReadStream();
                    streams.Add(stream);
                    return new CreateProjetImageDto
                    {
                        Title = Path.GetFileNameWithoutExtension(x.Fichier.FileName),
                        Est3D = x.Est3D,
                        FileStream = stream,
                        FileName = x.Fichier.FileName,
                        OrdreAffichage = i + 1,
                        EstPrincipale = i == 0
                    };
                }).ToList()
            };

            var cree = await _projetService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = cree.Id }, cree);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        finally { foreach (var s in streams) s.Dispose(); }
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(200_000_000)]
    public async Task<ActionResult<ProjetDto>> Update(string id, [FromForm] ProjetUpdateForm form)
    {
        var actuel = await _projetService.GetByIdAsync(id);
        if (actuel is null) return NotFound();

        var nouveaux = form.Images3D.Select(f => (Fichier: f, Est3D: true))
            .Concat(form.ImagesReelles.Select(f => (Fichier: f, Est3D: false)))
            .OrderBy(x => x.Fichier.FileName, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var erreur = ValiderFichiers(nouveaux.Select(x => x.Fichier));
        if (erreur is not null) return BadRequest(erreur);

        var ordreMax = actuel.Images.Count == 0 ? 0 : actuel.Images.Max(i => i.OrdreAffichage);
        var streams = new List<Stream>();
        try
        {
            // Images existantes : on les garde telles quelles (Id seul, Title/Est3D null = inchangés)
            var images = actuel.Images.Select(i => new UpdateProjetImageDto
            {
                Id = i.Id,
                OrdreAffichage = i.OrdreAffichage,
                EstPrincipale = i.EstPrincipale
            }).ToList();

            // Nouvelles images ajoutées à la fin
            images.AddRange(nouveaux.Select((x, i) =>
            {
                var stream = x.Fichier.OpenReadStream();
                streams.Add(stream);
                return new UpdateProjetImageDto
                {
                    Title = Path.GetFileNameWithoutExtension(x.Fichier.FileName),
                    Est3D = x.Est3D,
                    FileStream = stream,
                    FileName = x.Fichier.FileName,
                    OrdreAffichage = ordreMax + i + 1,
                    EstPrincipale = false
                };
            }));

            var maj = await _projetService.UpdateAsync(id, new UpdateProjetDto
            {
                Nom = form.Nom,
                Description = form.Description,
                Images = images
            });
            return maj is null ? NotFound() : Ok(maj);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        finally { foreach (var s in streams) s.Dispose(); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
        => await _projetService.DeleteAsync(id) ? NoContent() : NotFound();

    [HttpDelete("{id}/images/{imageId:int}")]
    public async Task<IActionResult> DeleteImage(string id, int imageId)
        => await _projetService.DeleteImageAsync(id, imageId) ? NoContent() : NotFound();

    // ---------- Validation ----------

    private static string? ValiderFormulaire(ProjetForm form, bool estCreation)
    {
        if (string.IsNullOrWhiteSpace(form.Nom))
            return "Le nom est obligatoire.";

        for (var i = 0; i < form.Images.Count; i++)
        {
            var img = form.Images[i];

            if (img.File is null)
            {
                if (estCreation || img.Id is null)
                    return $"L'image n°{i + 1} n'a pas de fichier.";
                continue;
            }

            if (img.File.Length == 0 || img.File.Length > TailleMaxImage)
                return $"L'image n°{i + 1} est vide ou dépasse 10 Mo.";

            if (!TypesAutorises.Contains(img.File.ContentType))
                return $"L'image n°{i + 1} doit être en JPEG, PNG ou WebP.";
        }

        return null;
    }

    private static string? ValiderFichiers(IEnumerable<IFormFile> fichiers)
    {
        foreach (var f in fichiers)
        {
            if (f.Length == 0 || f.Length > TailleMaxImage)
                return $"« {f.FileName} » est vide ou dépasse 10 Mo.";
            if (!TypesAutorises.Contains(f.ContentType))
                return $"« {f.FileName} » doit être en JPEG, PNG ou WebP.";
        }
        return null;
    }
}
