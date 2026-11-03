using ArtOfDesign.API.Extensions;
using ArtOfDesign.Application.DTOs.Couleurs;
using ArtOfDesign.Application.Interfaces.Couleurs;
using ArtOfDesign.Application.Services.Couleurs;
using Microsoft.AspNetCore.Mvc;

namespace ArtOfDesign.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CouleursController : ControllerBase
{
    private readonly ICouleurService _service;

    public CouleursController(ICouleurService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<CouleurDto>>> GetAll()
    {
        var couleurs = await _service.GetAllAsync();
        return Ok(couleurs);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CouleurDto>> GetById(int id)
    {
        var couleur = await _service.GetByIdAsync(id);

        if (couleur is null)
            return NotFound();

        return Ok(couleur);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateCouleurForm form)
    {
        if (form.Image == null || form.Image.Length == 0)
            return BadRequest(new { message = "Une image est obligatoire." });

        using var stream = form.Image.OpenReadStream();

        var dto = new CreateCouleurDto
        {
            Nom = form.Nom,
            CodeCouleur = form.CodeCouleur
        };

        var couleur = await _service.CreateAsync(
            dto,
            stream,
            form.Image.FileName
        );

        return CreatedAtAction(
            nameof(GetById),
            new { id = couleur.Id },
            couleur
        );
    }

    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<CouleurDto>> Update(
    int id,
    [FromForm] UpdateCouleurForm form)
    {
        using var stream = form.Image?.OpenReadStream();

        var dto = new UpdateCouleurDto
        {
            Nom = form.Nom,
            CodeCouleur = form.CodeCouleur
        };

        var couleur = await _service.UpdateAsync(
            id,
            dto,
            stream,
            form.Image?.FileName
        );

        if (couleur is null)
            return NotFound();

        return Ok(couleur);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}