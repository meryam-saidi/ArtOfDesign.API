using ArtOfDesign.Application.DTOs.Couleurs;
using ArtOfDesign.Application.Interfaces;
using ArtOfDesign.Application.Interfaces.Couleurs;
using ArtOfDesign.Domain.Entities.Catalogue;

namespace ArtOfDesign.Application.Services.Couleurs;

public class CouleurService : ICouleurService
{
    private readonly ICouleurRepository _repository;

    public CouleurService(ICouleurRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CouleurDto>> GetAllAsync()
    {
        var couleurs = await _repository.GetAllAsync();

        return couleurs.Select(c => new CouleurDto
        {
            Id = c.Id,
            Nom = c.Nom,
            CodeCouleur = c.CodeCouleur,
            ImagePresentationUrl = c.ImagePresentationUrl
        }).ToList();
    }

    public async Task<CouleurDto?> GetByIdAsync(int id)
    {
        var couleur = await _repository.GetByIdAsync(id);

        if (couleur is null)
            return null;

        return new CouleurDto
        {
            Id = couleur.Id,
            Nom = couleur.Nom,
            CodeCouleur = couleur.CodeCouleur,
            ImagePresentationUrl = couleur.ImagePresentationUrl
        };
    }

    public async Task<CouleurDto> CreateAsync(CreateCouleurDto dto)
    {
        var couleur = new Couleur
        {
            Nom = dto.Nom,
            CodeCouleur = dto.CodeCouleur,
            ImagePresentationUrl = dto.ImagePresentationUrl
        };

        await _repository.AddAsync(couleur);

        return new CouleurDto
        {
            Id = couleur.Id,
            Nom = couleur.Nom,
            CodeCouleur = couleur.CodeCouleur,
            ImagePresentationUrl = couleur.ImagePresentationUrl
        };
    }

    public async Task<CouleurDto?> UpdateAsync(int id, CouleurDto dto)
    {
        var couleur = await _repository.GetByIdAsync(id);

        if (couleur is null)
            return null;

        couleur.Nom = dto.Nom;
        couleur.CodeCouleur = dto.CodeCouleur;
        couleur.ImagePresentationUrl = dto.ImagePresentationUrl;

        await _repository.UpdateAsync(couleur);

        return new CouleurDto
        {
            Id = couleur.Id,
            Nom = couleur.Nom,
            CodeCouleur = couleur.CodeCouleur,
            ImagePresentationUrl = couleur.ImagePresentationUrl
        };
    }
}