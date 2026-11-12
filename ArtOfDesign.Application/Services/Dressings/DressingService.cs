using ArtOfDesign.Application.DTOs.Dressings;
using ArtOfDesign.Application.Interfaces;
using ArtOfDesign.Application.Interfaces.Dressings;
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Cuisines;
using ArtOfDesign.Domain.Entities.Dressings;


namespace ArtOfDesign.Application.Services.Dressings;

public class DressingService : IDressingService
{
    private readonly IDressingRepository _dressingRepository;
    private readonly IImageStorageService _imageStorageService;
    public DressingService(IDressingRepository dressingRepository, IImageStorageService imageStorageService)
    {
        _dressingRepository = dressingRepository;
        _imageStorageService = imageStorageService;
    }

    public async Task<List<DressingDto>> GetAllAsync()
    {
        var dressings = await _dressingRepository.GetAllAsync();

        return dressings.Select(c => new DressingDto
        {
            Id = c.Id,
            Categorie = c.Categorie,
            Forme = c.Forme,
            Nom = c.Nom,
            PrixInitial = c.PrixInitial,
            Reduction = c.Reduction,
            Description = c.Description,

            Dimensions = c.Dimensions == null ? null! : new DimensionsDressingDto
            {
                Hauteur = c.Dimensions.Hauteur,
                Profondeur = c.Dimensions.Profondeur,
                Largeur = c.Dimensions.Largeur
            },

            Images = c.Images
                .OrderBy(i => i.OrdreAffichage)
                .Select(i => new ImageDressingDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    OrdreAffichage = i.OrdreAffichage,
                    EstPrincipale = i.EstPrincipale
                })
                .ToList(),

            CaracteristiquesTechniques = new CaracteristiquesTechniquesDressingDto
            {
                CompositionType = c.CaracteristiquesTechniques.CompositionType,
                Caisson = c.CaracteristiquesTechniques.Caisson,
                Facade = c.CaracteristiquesTechniques.Facade
            },

            Couleurs = c.DressingCouleurs
                .Select(cc => new CouleurDressingDto
                {
                    Id = cc.Couleur.Id,
                    Nom = cc.Couleur.Nom,
                    CodeCouleur = cc.Couleur.CodeCouleur,
                    ImagePresentationUrl = cc.Couleur.ImagePresentationUrl
                })
                .ToList()
        }).ToList();
    }

    public async Task<DressingDto?> GetByIdAsync(string id)
    {
        var d = await _dressingRepository.GetByIdAsync(id);
        
        if (d == null)
            return null;

        return new DressingDto
        {
            Id = d.Id,
            Categorie = d.Categorie,
            Forme = d.Forme,
            Nom = d.Nom,
            PrixInitial = d.PrixInitial,
            Reduction = d.Reduction,
            Description = d.Description,

            Dimensions = d.Dimensions == null ? null! : new DimensionsDressingDto
            {
                Hauteur = d.Dimensions.Hauteur,
                Profondeur = d.Dimensions.Profondeur,
                Largeur = d.Dimensions.Largeur
            },

            Images = d.Images
                .OrderBy(i => i.OrdreAffichage)
                .Select(i => new ImageDressingDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    OrdreAffichage = i.OrdreAffichage,
                    EstPrincipale = i.EstPrincipale
                })
                .ToList(),

            CaracteristiquesTechniques = new CaracteristiquesTechniquesDressingDto
            {
                CompositionType = d.CaracteristiquesTechniques.CompositionType,
                Caisson = d.CaracteristiquesTechniques.Caisson,
                Facade = d.CaracteristiquesTechniques.Facade
            },

            Couleurs = d.DressingCouleurs
                .Select(cc => new CouleurDressingDto
                {
                    Id = cc.Couleur.Id,
                    Nom = cc.Couleur.Nom,
                    CodeCouleur = cc.Couleur.CodeCouleur,
                    ImagePresentationUrl = cc.Couleur.ImagePresentationUrl
                })
                .ToList()
        };
    }

    public async Task<DressingDto> CreateAsync(CreateDressingDto dto)
    {
        var images = new List<ImageDressing>();

        foreach (var imageDto in dto.Images)
        {
            var uploadResult = await _imageStorageService.UploadImageAsync(
                imageDto.FileStream,
                imageDto.FileName);

            images.Add(new ImageDressing
            {
                Url = uploadResult.Url,
                PublicId = uploadResult.PublicId,
                OrdreAffichage = imageDto.OrdreAffichage,
                EstPrincipale = imageDto.EstPrincipale
            });
        }

        var dressing = new Dressing
        {
            Id = dto.Id,
            Categorie = "Dressings",
            Forme = dto.Forme,
            Nom = dto.Nom,
            PrixInitial = dto.PrixInitial,
            Reduction = dto.Reduction,
            Description = dto.Description,

            Dimensions = new DimensionsDressing
            {
                Hauteur = dto.Dimensions.Hauteur,
                Profondeur = dto.Dimensions.Profondeur,
                Largeur = dto.Dimensions.Largeur
            },

            Images = images,

            CaracteristiquesTechniques = new CaracteristiquesTechniquesDressing
            {
                CompositionType = dto.CaracteristiquesTechniques.CompositionType,
                Caisson = dto.CaracteristiquesTechniques.Caisson,
                Facade = dto.CaracteristiquesTechniques.Facade
            },

            DressingCouleurs = dto.CouleurIds
                .Distinct()
                .Select(id => new DressingCouleur
                {
                    CouleurId = id
                })
                .ToList()
        };

        await _dressingRepository.AddAsync(dressing);

        var dressingcree = await _dressingRepository.GetByIdAsync(dressing.Id);
        
        if (dressingcree is null) throw new InvalidOperationException(
                "Le Dressing a été créé, mais sa récupération a échoué.");

        return new DressingDto
        {
            Id = dressingcree.Id.ToString(),
            Categorie = dressingcree.Categorie,
            Forme = dressingcree.Forme,
            Nom = dressingcree.Nom,
            PrixInitial = dressingcree.PrixInitial,
            Reduction = dressingcree.Reduction,
            Description = dressingcree.Description,

            Dimensions = dressingcree.Dimensions == null ? null! : new DimensionsDressingDto
            {
                Hauteur = dressingcree.Dimensions.Hauteur,
                Profondeur = dressingcree.Dimensions.Profondeur,
                Largeur = dressingcree.Dimensions.Largeur
            },

            Images = dressingcree.Images
                .OrderBy(i => i.OrdreAffichage)
                .Select(i => new ImageDressingDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    OrdreAffichage = i.OrdreAffichage,
                    EstPrincipale = i.EstPrincipale
                })
                .ToList(),

            CaracteristiquesTechniques = new CaracteristiquesTechniquesDressingDto
            {
                CompositionType = dressingcree.CaracteristiquesTechniques.CompositionType,
                Caisson = dressingcree.CaracteristiquesTechniques.Caisson,
                Facade = dressingcree.CaracteristiquesTechniques.Facade
            },

            Couleurs = dressingcree.DressingCouleurs
                .Select(cc => new CouleurDressingDto
                {
                    Id = cc.Couleur.Id,
                    Nom = cc.Couleur.Nom,
                    CodeCouleur = cc.Couleur.CodeCouleur,
                    ImagePresentationUrl = cc.Couleur.ImagePresentationUrl
                })
                .ToList()
        };
    }

    public Task<bool> DeleteImageAsync(string DressingId, int imageId)
    {
        throw new NotImplementedException();
    }

    public Task<DressingDto?> UpdateAsync(string id, UpdateDressingDto dto)
    {
        throw new NotImplementedException();
    }
}
