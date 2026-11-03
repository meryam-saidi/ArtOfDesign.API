using ArtOfDesign.Application.DTOs.Couleurs;
using ArtOfDesign.Application.Interfaces;
using ArtOfDesign.Application.Interfaces.Couleurs;
using ArtOfDesign.Domain.Entities.Catalogue;

namespace ArtOfDesign.Application.Services.Couleurs;

public class CouleurService : ICouleurService
{
    private readonly ICouleurRepository _repository;
    private readonly IImageStorageService _imageStorageService;

    public CouleurService(ICouleurRepository repository, IImageStorageService imageStorageService)
    {
        _repository = repository;
        _imageStorageService = imageStorageService;
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

    public async Task<CouleurDto> CreateAsync(CreateCouleurDto dto, Stream imageStream, string fileName)
    {
        // 1. Téléverser l'image sur Cloudinary
        var uploadResult = await _imageStorageService.UploadImageAsync(
            imageStream,
            fileName
        );

        // 2. Créer l'entité Couleur
        var couleur = new Couleur
        {
            Nom = dto.Nom,
            PublicId = uploadResult.PublicId,
            CodeCouleur = dto.CodeCouleur,
            ImagePresentationUrl = uploadResult.Url
        };

        // 3. Enregistrer la couleur en base de données
        await _repository.AddAsync(couleur);

        // 4. Retourner le DTO
        return new CouleurDto
        {
            Id = couleur.Id,
            Nom = couleur.Nom,
            CodeCouleur = couleur.CodeCouleur,
            ImagePresentationUrl = couleur.ImagePresentationUrl
        };
    }

    public async Task<CouleurDto?> UpdateAsync(
    int id,
    UpdateCouleurDto dto,
    Stream? imageStream,
    string? fileName)
    {
        var couleur = await _repository.GetByIdAsync(id);

        if (couleur is null)
            return null;

        couleur.Nom = dto.Nom;
        couleur.CodeCouleur = dto.CodeCouleur;

        if (imageStream != null && !string.IsNullOrEmpty(fileName))
        {
            if (!string.IsNullOrEmpty(couleur.PublicId))
                await _imageStorageService.DeleteImageAsync(couleur.PublicId);

            var uploadResult = await _imageStorageService.UploadImageAsync(
                imageStream,
                fileName
            );

            couleur.ImagePresentationUrl = uploadResult.Url;
            couleur.PublicId = uploadResult.PublicId;
        }

        await _repository.UpdateAsync(couleur);

        return new CouleurDto
        {
            Id = couleur.Id,
            Nom = couleur.Nom,
            CodeCouleur = couleur.CodeCouleur,
            ImagePresentationUrl = couleur.ImagePresentationUrl
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var couleur = await _repository.GetByIdAsync(id);

        if (couleur is null)
            return false;

        // Supprimer l'image de Cloudinary
        if (!string.IsNullOrEmpty(couleur.PublicId))
        {
            await _imageStorageService.DeleteImageAsync(couleur.PublicId);
        }

        // Supprimer la couleur de la base de données
        await _repository.DeleteAsync(couleur);

        return true;
    }
}