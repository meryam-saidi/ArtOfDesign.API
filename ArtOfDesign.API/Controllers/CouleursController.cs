using ArtOfDesign.Application.DTOs.Couleurs;
using ArtOfDesign.Application.Interfaces.Couleurs;
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
    public async Task<ActionResult<CouleurDto>> Create(
        [FromBody] CreateCouleurDto dto)
    {
        var couleur = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = couleur.Id },
            couleur);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CouleurDto>> Update(
        int id,
        [FromBody] CouleurDto dto)
    {
        var couleur = await _service.UpdateAsync(id, dto);

        if (couleur is null)
            return NotFound();

        return Ok(couleur);
    }
}