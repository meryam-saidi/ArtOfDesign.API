using ArtOfDesign.Application.DTOs.Cuisines;
using ArtOfDesign.Application.DTOs.Dressings;
using ArtOfDesign.Application.Interfaces.Dressings;
using Microsoft.AspNetCore.Mvc;

namespace ArtOfDesign.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DressingsController : ControllerBase
{
    private readonly IDressingService _DressingService;

    public DressingsController(IDressingService DressingService)
    {
        _DressingService = DressingService;
    }

    [HttpGet]
    public async Task<ActionResult<List<DressingDto>>> GetAll()
    {
        var Dressings = await _DressingService.GetAllAsync();
        return Ok(Dressings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DressingDto>> GetById(string id)
    {
        var Dressing = await _DressingService.GetByIdAsync(id);

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

            var cuisine = await _DressingService.CreateAsync(dto);

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
}
