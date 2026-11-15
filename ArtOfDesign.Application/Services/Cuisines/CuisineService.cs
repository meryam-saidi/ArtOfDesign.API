
using ArtOfDesign.Application.DTOs.Cuisines;
using ArtOfDesign.Application.DTOs.Cuisines.Create;
using ArtOfDesign.Application.DTOs.Cuisines.Patch;
using ArtOfDesign.Application.DTOs.Cuisines.Update;
using ArtOfDesign.Application.DTOs.Cuisines.Update;
using ArtOfDesign.Application.DTOs.Cuisines;
using ArtOfDesign.Application.Interfaces;
using ArtOfDesign.Application.Interfaces.Cuisines;
using ArtOfDesign.Application.Interfaces.Cuisines;
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Cuisines;
using ArtOfDesign.Domain.Entities.Cuisines;

namespace ArtOfDesign.Application.Services.Cuisines;

public class CuisineService : ICuisineService
{
    private readonly ICuisineRepository _repository;
    private readonly IImageStorageService _imageStorageService;

    public CuisineService(ICuisineRepository repository, IImageStorageService imageStorageService)
    {
        _repository = repository;
        _imageStorageService = imageStorageService;
    }

    private static CuisineDto MapToDto(Cuisine c)
    {
        return new CuisineDto
        {
            Id = c.Id,
            Categorie = c.Categorie,
            Forme = c.Forme,
            Nom = c.Nom,
            PrixInitial = c.PrixInitial,
            Reduction = c.Reduction,
            Description = c.Description,
            Nb = c.Nb,

            Dimensions = new DimensionsCuisineDto
            {
                HauteurCaissonBas = c.Dimensions.HauteurCaissonBas,
                ProfondeurCaissonBas = c.Dimensions.ProfondeurCaissonBas,
                HauteurCaissonHaut = c.Dimensions.HauteurCaissonHaut,
                ProfondeurCaissonHaut = c.Dimensions.ProfondeurCaissonHaut
            },

            Images = c.Images
                .OrderBy(i => i.OrdreAffichage)
                .Select(i => new ImageCuisineDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    OrdreAffichage = i.OrdreAffichage,
                    EstPrincipale = i.EstPrincipale
                })
                .ToList(),

            CaracteristiquesTechniques = new CaracteristiquesTechniquesCuisineDto
            {
                CompositionType = c.CaracteristiquesTechniques.CompositionType,
                Caisson = c.CaracteristiquesTechniques.Caisson,
                Facade = c.CaracteristiquesTechniques.Facade
            },

            Couleurs = c.CuisineCouleurs
                .Select(cc => new CouleurCuisineDto
                {
                    Id = cc.Couleur.Id,
                    Nom = cc.Couleur.Nom,
                    CodeCouleur = cc.Couleur.CodeCouleur,
                    ImagePresentationUrl = cc.Couleur.ImagePresentationUrl
                })
                .ToList()
        };
    }

    public async Task<List<CuisineDto>> GetAllAsync()
    {
        var cuisines = await _repository.GetAllAsync();

        return cuisines.Select(MapToDto).ToList();
    }

    public async Task<CuisineDto?> GetByIdAsync(string id)
    {
        var c = await _repository.GetByIdAsync(id);

        if (c is null)
            return null;

        return MapToDto(c);
    }

    public async Task<CuisineDto> CreateAsync(CreateCuisineDto dto)
    {
        var images = new List<ImageCuisine>();

        foreach (var imageDto in dto.Images)
        {
            var uploadResult = await _imageStorageService.UploadImageAsync(
                imageDto.FileStream,
                imageDto.FileName);

            images.Add(new ImageCuisine
            {
                Url = uploadResult.Url,
                PublicId = uploadResult.PublicId,
                OrdreAffichage = imageDto.OrdreAffichage,
                EstPrincipale = imageDto.EstPrincipale
            });
        }

        var cuisine = new Cuisine
        {
            Id = dto.Id,
            Categorie = "Cuisines",
            Forme = dto.Forme,
            Nom = dto.Nom,
            PrixInitial = dto.PrixInitial,
            Reduction = dto.Reduction,
            Description = dto.Description,
            Nb = dto.Nb,

            Dimensions = new DimensionsCuisine
            {
                HauteurCaissonBas = dto.Dimensions.HauteurCaissonBas,
                ProfondeurCaissonBas = dto.Dimensions.ProfondeurCaissonBas,
                HauteurCaissonHaut = dto.Dimensions.HauteurCaissonHaut,
                ProfondeurCaissonHaut = dto.Dimensions.ProfondeurCaissonHaut
            },

            CaracteristiquesTechniques = new CaracteristiquesTechniquesCuisine
            {
                CompositionType = dto.CaracteristiquesTechniques.CompositionType,
                Caisson = dto.CaracteristiquesTechniques.Caisson,
                Facade = dto.CaracteristiquesTechniques.Facade
            },

            Images = images,

            CuisineCouleurs = dto.CouleurIds
                .Distinct()
                .Select(id => new CuisineCouleur
                {
                    CouleurId = id
                })
                .ToList()
        };

        await _repository.AddAsync(cuisine);

        var cuisineCreee = await _repository.GetByIdAsync(cuisine.Id);

        if (cuisineCreee is null)
            throw new InvalidOperationException(
                "La cuisine a été créée, mais sa récupération a échoué.");

        return MapToDto(cuisineCreee);
    }

    public async Task<CuisineDto?> UpdateAsync(string id, UpdateCuisineDto dto)
    {
        // 1. Récupérer le Cuisine existant AVEC toutes ses relations (via le repository)
        var cuisine = await _repository.GetByIdAsync(id);
        if (cuisine == null)
            return null; // Le Cuisine n'existe pas -> le contrôleur renverra un 404

        // 2. Mettre à jour les propriétés simples de l'entité Cuisine
        cuisine.Categorie = dto.Categorie;
        cuisine.Forme = dto.Forme;
        cuisine.Nom = dto.Nom;
        cuisine.PrixInitial = dto.PrixInitial;
        cuisine.Reduction = dto.Reduction;
        cuisine.Description = dto.Description;
        cuisine.Nb = dto.Nb;

        // 3. Mettre à jour les Dimensions (comme c'est un objet 1-to-1, on met à jour ses propriétés)
        if (cuisine.Dimensions != null)
        {
            cuisine.Dimensions.HauteurCaissonBas = dto.Dimensions.HauteurCaissonBas;
            cuisine.Dimensions.ProfondeurCaissonBas = dto.Dimensions.ProfondeurCaissonBas;
            cuisine.Dimensions.HauteurCaissonBas = dto.Dimensions.HauteurCaissonHaut;
            cuisine.Dimensions.ProfondeurCaissonHaut = dto.Dimensions.ProfondeurCaissonHaut;
        }
        else
        {
            // Sécurité au cas où elles n'existeraient pas
            cuisine.Dimensions = new DimensionsCuisine
            {
                HauteurCaissonBas = dto.Dimensions.HauteurCaissonBas,
                ProfondeurCaissonBas = dto.Dimensions.ProfondeurCaissonBas,
                HauteurCaissonHaut = dto.Dimensions.HauteurCaissonHaut,
                ProfondeurCaissonHaut = dto.Dimensions.ProfondeurCaissonHaut
            };
        }

        // 4. Mettre à jour les Caractéristiques Techniques
        if (cuisine.CaracteristiquesTechniques != null)
        {
            cuisine.CaracteristiquesTechniques.CompositionType = dto.CaracteristiquesTechniques.CompositionType;
            cuisine.CaracteristiquesTechniques.Caisson = dto.CaracteristiquesTechniques.Caisson;
            cuisine.CaracteristiquesTechniques.Facade = dto.CaracteristiquesTechniques.Facade;
        }
        else
        {
            cuisine.CaracteristiquesTechniques = new CaracteristiquesTechniquesCuisine
            {
                CompositionType = dto.CaracteristiquesTechniques.CompositionType,
                Caisson = dto.CaracteristiquesTechniques.Caisson,
                Facade = dto.CaracteristiquesTechniques.Facade
            };
        }

        // 5. Gérer les Couleurs (Relation Many-to-Many via CuisineCouleurs)
        // La stratégie la plus propre en PUT : On vide la liste actuelle et on recrée les associations demandées
        cuisine.CuisineCouleurs.Clear();
        foreach (var couleurId in dto.CouleurIds.Distinct())
        {
            cuisine.CuisineCouleurs.Add(new CuisineCouleur
            {
                CuisineId = cuisine.Id,
                CouleurId = couleurId
            });
        }

        // 6. Gérer les Images (Attention au stockage physique/cloud et aux images existantes)
        // Ici, on gère le cas où l'utilisateur envoie de nouvelles images ou garde les existantes.
        var updatedImages = new List<ImageCuisine>();

        foreach (var imageDto in dto.Images)
        {
            // CAS 1 : C'est une NOUVELLE image envoyée sous forme de fichier
            if (imageDto.FileStream != null && imageDto.FileStream.Length > 0)
            {
                var uploadResult = await _imageStorageService.UploadImageAsync(
                    imageDto.FileStream,
                    imageDto.FileName);

                updatedImages.Add(new ImageCuisine
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
                updatedImages.Add(new ImageCuisine
                {
                    Id = imageDto.Id.Value, // On garde l'ID existant
                    Url = imageDto.Url,     // On garde l'URL existante
                    OrdreAffichage = imageDto.OrdreAffichage,
                    EstPrincipale = imageDto.EstPrincipale
                });
            }
        }

        // On remplace l'ancienne liste par la nouvelle
        cuisine.Images = updatedImages;

        // 7. Sauvegarder via le Repository
        await _repository.UpdateAsync(cuisine);

        // 8. Re-récupérer proprement l'objet mis à jour pour garantir l'intégrité (exactement comme dans Create)
        var cuisineMisAJour = await _repository.GetByIdAsync(cuisine.Id);
        if (cuisineMisAJour is null)
            throw new InvalidOperationException("Le Cuisine a été mis à jour, mais sa récupération a échoué.");

        // 9. Retourner le DTO formaté
        return MapToDto(cuisineMisAJour);
    }

    public async Task<CuisineDto?> PatchAsync(
    string id,
    PatchCuisineDto dto,
    List<CreateCuisineImageDto> nouvellesImages)
    {
        var cuisine = await _repository.GetByIdAsync(id);

        if (cuisine is null)
            return null;

        // 1. Informations principales
        if (dto.Categorie is not null)
            cuisine.Categorie = dto.Categorie;

        if (dto.Forme is not null)
            cuisine.Forme = dto.Forme;

        if (dto.Nom is not null)
            cuisine.Nom = dto.Nom;

        if (dto.PrixInitial.HasValue)
            cuisine.PrixInitial = dto.PrixInitial.Value;

        if (dto.Reduction.HasValue)
            cuisine.Reduction = dto.Reduction.Value;

        if (dto.Description is not null)
            cuisine.Description = dto.Description;

        if (dto.Nb is not null)
            cuisine.Nb = dto.Nb;

        // 2. Dimensions
        if (dto.Dimensions is not null)
        {
            cuisine.Dimensions ??= new DimensionsCuisine
            {
                CuisineId = cuisine.Id
            };

            var dimensions = dto.Dimensions;

            if (dimensions.HauteurCaissonBas is not null)
                cuisine.Dimensions.HauteurCaissonBas =
                    dimensions.HauteurCaissonBas;

            if (dimensions.ProfondeurCaissonBas is not null)
                cuisine.Dimensions.ProfondeurCaissonBas =
                    dimensions.ProfondeurCaissonBas;

            if (dimensions.HauteurCaissonHaut is not null)
                cuisine.Dimensions.HauteurCaissonHaut =
                    dimensions.HauteurCaissonHaut;

            if (dimensions.ProfondeurCaissonHaut is not null)
                cuisine.Dimensions.ProfondeurCaissonHaut =
                    dimensions.ProfondeurCaissonHaut;
        }

        // 3. Caractéristiques techniques
        if (dto.CaracteristiquesTechniques is not null)
        {
            cuisine.CaracteristiquesTechniques ??=
                new CaracteristiquesTechniquesCuisine
                {
                    CuisineId = cuisine.Id
                };

            var caracteristiques = dto.CaracteristiquesTechniques;

            if (caracteristiques.CompositionType is not null)
                cuisine.CaracteristiquesTechniques.CompositionType =
                    caracteristiques.CompositionType;

            if (caracteristiques.Caisson is not null)
                cuisine.CaracteristiquesTechniques.Caisson =
                    caracteristiques.Caisson;

            if (caracteristiques.Facade is not null)
                cuisine.CaracteristiquesTechniques.Facade =
                    caracteristiques.Facade;
        }

        // 4. Couleurs
        if (dto.CouleurIds is not null)
        {
            var nouvellesCouleurs = dto.CouleurIds.Distinct().ToHashSet();

            var couleursExistantes = cuisine.CuisineCouleurs
                .Select(cc => cc.CouleurId)
                .ToHashSet();

            // Retirer uniquement les associations qui ne sont plus sélectionnées
            var associationsASupprimer = cuisine.CuisineCouleurs
                .Where(cc => !nouvellesCouleurs.Contains(cc.CouleurId))
                .ToList();

            foreach (var association in associationsASupprimer)
                cuisine.CuisineCouleurs.Remove(association);

            // Ajouter uniquement les nouvelles associations
            foreach (var couleurId in nouvellesCouleurs.Except(couleursExistantes))
            {
                cuisine.CuisineCouleurs.Add(new CuisineCouleur
                {
                    CuisineId = cuisine.Id,
                    CouleurId = couleurId
                });
            }
        }

        // 5. Ajouter les nouvelles images sans supprimer les anciennes
        foreach (var imageDto in nouvellesImages)
        {
            var prochaineOrdre = cuisine.Images.Count == 0
                ? 1
                : cuisine.Images.Max(i => i.OrdreAffichage) + 1;

            var estPrincipale = !cuisine.Images.Any(i => i.EstPrincipale);

            var uploadResult = await _imageStorageService.UploadImageAsync(
                imageDto.FileStream,
                imageDto.FileName);

            cuisine.Images.Add(new ImageCuisine
            {
                Url = uploadResult.Url,
                PublicId = uploadResult.PublicId,
                OrdreAffichage = prochaineOrdre,
                EstPrincipale = estPrincipale,
                CuisineId = cuisine.Id
            });
        }

        await _repository.UpdateAsync(cuisine);

        return MapToDto(cuisine);
    }

    public async Task<bool> DeleteImageAsync(string cuisineId, int imageId)
    {
        var cuisine = await _repository.GetByIdAsync(cuisineId);

        if (cuisine is null)
            return false;

        var image = cuisine.Images.FirstOrDefault(i => i.Id == imageId);

        if (image is null)
            return false;

        // 1. Supprimer l'image de Cloudinary si son PublicId existe
        if (!string.IsNullOrWhiteSpace(image.PublicId))
        {
            await _imageStorageService.DeleteImageAsync(image.PublicId);
        }

        // 2. Retirer l'image de la collection
        cuisine.Images.Remove(image);

        // 3. Si elle était principale, choisir une autre image
        if (image.EstPrincipale)
        {
            var nouvelleImagePrincipale = cuisine.Images
                .OrderBy(i => i.OrdreAffichage)
                .FirstOrDefault();

            if (nouvelleImagePrincipale is not null)
                nouvelleImagePrincipale.EstPrincipale = true;
        }

        // 4. Enregistrer la suppression en base
        await _repository.DeleteImageAsync(image);

        return true;
    } 
}
