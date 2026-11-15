using ArtOfDesign.Application.DTOs.Dressings;
using ArtOfDesign.Application.DTOs.Dressings.Create;
using ArtOfDesign.Application.DTOs.Dressings.Update;
using ArtOfDesign.Application.Interfaces;
using ArtOfDesign.Application.Interfaces.Dressings;
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Cuisines;
using ArtOfDesign.Domain.Entities.Dressings;
using static System.Net.Mime.MediaTypeNames;


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

    private static DressingDto MapToDto(Dressing d)
    {
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

        return MapToDto(dressingcree);
    }


    public async Task<DressingDto?> UpdateAsync(string id, UpdateDressingDto dto)
    {
        // 1. Récupérer le dressing existant AVEC toutes ses relations (via le repository)
        var dressing = await _dressingRepository.GetByIdAsync(id);
        if (dressing == null)
            return null; // Le dressing n'existe pas -> le contrôleur renverra un 404

        // 2. Mettre à jour les propriétés simples de l'entité Dressing
        dressing.Categorie = dto.Categorie;
        dressing.Forme = dto.Forme;
        dressing.Nom = dto.Nom;
        dressing.PrixInitial = dto.PrixInitial;
        dressing.Reduction = dto.Reduction;
        dressing.Description = dto.Description;

        // 3. Mettre à jour les Dimensions (comme c'est un objet 1-to-1, on met à jour ses propriétés)
        if (dressing.Dimensions != null)
        {
            dressing.Dimensions.Hauteur = dto.Dimensions.Hauteur;
            dressing.Dimensions.Profondeur = dto.Dimensions.Profondeur;
            dressing.Dimensions.Largeur = dto.Dimensions.Largeur;
        }
        else
        {
            // Sécurité au cas où elles n'existeraient pas
            dressing.Dimensions = new DimensionsDressing
            {
                Hauteur = dto.Dimensions.Hauteur,
                Profondeur = dto.Dimensions.Profondeur,
                Largeur = dto.Dimensions.Largeur
            };
        }

        // 4. Mettre à jour les Caractéristiques Techniques
        if (dressing.CaracteristiquesTechniques != null)
        {
            dressing.CaracteristiquesTechniques.CompositionType = dto.CaracteristiquesTechniques.CompositionType;
            dressing.CaracteristiquesTechniques.Caisson = dto.CaracteristiquesTechniques.Caisson;
            dressing.CaracteristiquesTechniques.Facade = dto.CaracteristiquesTechniques.Facade;
        }
        else
        {
            dressing.CaracteristiquesTechniques = new CaracteristiquesTechniquesDressing
            {
                CompositionType = dto.CaracteristiquesTechniques.CompositionType,
                Caisson = dto.CaracteristiquesTechniques.Caisson,
                Facade = dto.CaracteristiquesTechniques.Facade
            };
        }

        // 5. Gérer les Couleurs (Relation Many-to-Many via DressingCouleurs)
        // La stratégie la plus propre en PUT : On vide la liste actuelle et on recrée les associations demandées
        dressing.DressingCouleurs.Clear();
        foreach (var couleurId in dto.CouleurIds.Distinct())
        {
            dressing.DressingCouleurs.Add(new DressingCouleur
            {
                DressingId = dressing.Id,
                CouleurId = couleurId
            });
        }

        // 6. Gérer les Images (Attention au stockage physique/cloud et aux images existantes)
        // Ici, on gère le cas où l'utilisateur envoie de nouvelles images ou garde les existantes.
        var updatedImages = new List<ImageDressing>();

        foreach (var imageDto in dto.Images)
        {
            // CAS 1 : C'est une NOUVELLE image envoyée sous forme de fichier
            if (imageDto.FileStream != null && imageDto.FileStream.Length > 0)
            {
                var uploadResult = await _imageStorageService.UploadImageAsync(
                    imageDto.FileStream,
                    imageDto.FileName);

                updatedImages.Add(new ImageDressing
                {
                    Url = uploadResult.Url,
                    PublicId = uploadResult.PublicId,
                    OrdreAffichage = imageDto.OrdreAffichage,
                    EstPrincipale = imageDto.EstPrincipale
                });
            }
            // CAS 2 : C'est une image EXISTANTE que l'on souhaite conserver
            else if (imageDto.Id.HasValue && !string.IsNullOrEmpty(imageDto.Url))
            {
                updatedImages.Add(new ImageDressing
                {
                    Id = imageDto.Id.Value, // On garde l'ID existant
                    Url = imageDto.Url,     // On garde l'URL existante
                    OrdreAffichage = imageDto.OrdreAffichage,
                    EstPrincipale = imageDto.EstPrincipale
                });
            }
        }

        // On remplace l'ancienne liste par la nouvelle
        dressing.Images = updatedImages;

        // 7. Sauvegarder via le Repository
        await _dressingRepository.UpdateAsync(dressing);

        // 8. Re-récupérer proprement l'objet mis à jour pour garantir l'intégrité (exactement comme dans Create)
        var dressingMisAJour = await _dressingRepository.GetByIdAsync(dressing.Id);
        if (dressingMisAJour is null)
            throw new InvalidOperationException("Le Dressing a été mis à jour, mais sa récupération a échoué.");

        // 9. Retourner le DTO formaté
        return MapToDto(dressingMisAJour);
    }



    public async Task<bool> DeleteImageAsync(string dressingId, int imageId)
    {
        var dressing = await _dressingRepository.GetByIdAsync(dressingId);

        if (dressing is null)
            return false;

        var image = dressing.Images.FirstOrDefault(i => i.Id == imageId);

        if (image is null)
            return false;

        // 1. Supprimer l'image de Cloudinary si son PublicId existe
        if (!string.IsNullOrWhiteSpace(image.PublicId))
        {
            await _imageStorageService.DeleteImageAsync(image.PublicId);
        }

        // 2. Retirer l'image de la collection
        dressing.Images.Remove(image);

        // 3. Si elle était principale, choisir une autre image
        if (image.EstPrincipale)
        {
            var nouvelleImagePrincipale = dressing.Images
                .OrderBy(i => i.OrdreAffichage)
                .FirstOrDefault();

            if (nouvelleImagePrincipale is not null)
                nouvelleImagePrincipale.EstPrincipale = true;
        }

        // 4. Enregistrer la suppression en base
        await _dressingRepository.DeleteImageAsync(image);

        return true;
    }
}
