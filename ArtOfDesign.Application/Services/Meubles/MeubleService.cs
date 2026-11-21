using ArtOfDesign.Application.Interfaces;
using ArtOfDesign.Application.Interfaces.Meubles;
using ArtOfDesign.Application.DTOs.Meubles;
using ArtOfDesign.Application.DTOs.Meubles.Create;
using ArtOfDesign.Application.DTOs.Meubles.Update;
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Meubles;


namespace ArtOfDesign.Application.Services.Meubles;

public class MeubleService : IMeubleService
{
    private readonly IMeubleRepository _meubleRepository;
    private readonly IImageStorageService _imageStorageService;
    public MeubleService(IMeubleRepository meubleRepository, IImageStorageService imageStorageService)
    {
        _meubleRepository = meubleRepository;
        _imageStorageService = imageStorageService;
    }

    private static MeubleDto MapToDto(Meuble m)
    {
        return new MeubleDto
        {
            Id = m.Id,
            Categorie = m.Categorie,
            Forme = m.Forme,
            Nom = m.Nom,
            PrixInitial = m.PrixInitial,
            Reduction = m.Reduction,
            Description = m.Description,

            Dimensions = m.Dimensions == null ? null! : new DimensionsMeubleDto
            {
                Hauteur = m.Dimensions.Hauteur,
                Profondeur = m.Dimensions.Profondeur,
                Largeur = m.Dimensions.Largeur
            },

            Images = m.Images
                .OrderBy(i => i.OrdreAffichage)
                .Select(i => new ImageMeubleDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    OrdreAffichage = i.OrdreAffichage,
                    EstPrincipale = i.EstPrincipale
                })
                .ToList(),

            CaracteristiquesTechniques = new CaracteristiquesTechniquesMeubleDto
            {
                CompositionType = m.CaracteristiquesTechniques.CompositionType,
                Caisson = m.CaracteristiquesTechniques.Caisson,
                Facade = m.CaracteristiquesTechniques.Facade
            },

            Couleurs = m.MeubleCouleurs
                .Select(cc => new CouleurMeubleDto
                {
                    Id = cc.Couleur.Id,
                    Nom = cc.Couleur.Nom,
                    CodeCouleur = cc.Couleur.CodeCouleur,
                    ImagePresentationUrl = cc.Couleur.ImagePresentationUrl
                })
                .ToList()
        };
    }

    public async Task<MeubleDto> CreateAsync(CreateMeubleDto dto)
    {
        var images = new List<ImageMeuble>();

        foreach (var imageDto in dto.Images)
        {
            var uploadResult = await _imageStorageService.UploadImageAsync(
                imageDto.FileStream,
                imageDto.FileName);

            images.Add(new ImageMeuble
            {
                Url = uploadResult.Url,
                PublicId = uploadResult.PublicId,
                OrdreAffichage = imageDto.OrdreAffichage,
                EstPrincipale = imageDto.EstPrincipale
            });
        }

        var meuble = new Meuble
        {
            Id = dto.Id,
            Categorie = "Meubles",
            Forme = dto.Forme,
            Nom = dto.Nom,
            PrixInitial = dto.PrixInitial,
            Reduction = dto.Reduction,
            Description = dto.Description,

            Dimensions = new DimensionsMeuble
            {
                Hauteur = dto.Dimensions.Hauteur,
                Profondeur = dto.Dimensions.Profondeur,
                Largeur = dto.Dimensions.Largeur
            },

            Images = images,

            CaracteristiquesTechniques = new CaracteristiquesTechniquesMeuble
            {
                CompositionType = dto.CaracteristiquesTechniques.CompositionType,
                Caisson = dto.CaracteristiquesTechniques.Caisson,
                Facade = dto.CaracteristiquesTechniques.Facade
            },

            MeubleCouleurs = dto.CouleurIds
                .Distinct()
                .Select(id => new MeubleCouleur
                {
                    CouleurId = id
                })
            .ToList()
        };

        await _meubleRepository.AddAsync(meuble);

        var meublecree = await _meubleRepository.GetByIdAsync(meuble.Id);

        if (meublecree is null) throw new InvalidOperationException(
                "Le Meuble a été créé, mais sa récupération a échoué.");

        return MapToDto(meublecree);
    }

    public async Task<bool> DeleteImageAsync(string meubleId, int imageId)
    {
        var meuble = await _meubleRepository.GetByIdAsync(meubleId);

        if (meuble is null)
            return false;

        var image = meuble.Images.FirstOrDefault(i => i.Id == imageId);

        if (image is null)
            return false;

        // 1. Supprimer l'image de Cloudinary si son PublicId existe
        if (!string.IsNullOrWhiteSpace(image.PublicId))
        {
            await _imageStorageService.DeleteImageAsync(image.PublicId);
        }

        // 2. Retirer l'image de la collection
        meuble.Images.Remove(image);

        // 3. Si elle était principale, choisir une autre image
        if (image.EstPrincipale)
        {
            var nouvelleImagePrincipale = meuble.Images
                .OrderBy(i => i.OrdreAffichage)
                .FirstOrDefault();

            if (nouvelleImagePrincipale is not null)
                nouvelleImagePrincipale.EstPrincipale = true;
        }

        // 4. Enregistrer la suppression en base
        await _meubleRepository.DeleteImageAsync(image);

        return true;
    }

    public async Task<List<MeubleDto>> GetAllAsync()
    {
        var meubles = await _meubleRepository.GetAllAsync();

        return meubles.Select(c => new MeubleDto
        {
            Id = c.Id,
            Categorie = c.Categorie,
            Forme = c.Forme,
            Nom = c.Nom,
            PrixInitial = c.PrixInitial,
            Reduction = c.Reduction,
            Description = c.Description,

            Dimensions = c.Dimensions == null ? null! : new DimensionsMeubleDto
            {
                Hauteur = c.Dimensions.Hauteur,
                Profondeur = c.Dimensions.Profondeur,
                Largeur = c.Dimensions.Largeur
            },

            Images = c.Images
                .OrderBy(i => i.OrdreAffichage)
                .Select(i => new ImageMeubleDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    OrdreAffichage = i.OrdreAffichage,
                    EstPrincipale = i.EstPrincipale
                })
                .ToList(),

            CaracteristiquesTechniques = new CaracteristiquesTechniquesMeubleDto
            {
                CompositionType = c.CaracteristiquesTechniques.CompositionType,
                Caisson = c.CaracteristiquesTechniques.Caisson,
                Facade = c.CaracteristiquesTechniques.Facade
            },

            Couleurs = c.MeubleCouleurs
                .Select(cc => new CouleurMeubleDto
                {
                    Id = cc.Couleur.Id,
                    Nom = cc.Couleur.Nom,
                    CodeCouleur = cc.Couleur.CodeCouleur,
                    ImagePresentationUrl = cc.Couleur.ImagePresentationUrl
                })
                .ToList()
        }).ToList();
    }

    public async Task<MeubleDto?> GetByIdAsync(string id)
    {
        var d = await _meubleRepository.GetByIdAsync(id);

        if (d == null)
            return null;

        return new MeubleDto
        {
            Id = d.Id,
            Categorie = d.Categorie,
            Forme = d.Forme,
            Nom = d.Nom,
            PrixInitial = d.PrixInitial,
            Reduction = d.Reduction,
            Description = d.Description,

            Dimensions = d.Dimensions == null ? null! : new DimensionsMeubleDto
            {
                Hauteur = d.Dimensions.Hauteur,
                Profondeur = d.Dimensions.Profondeur,
                Largeur = d.Dimensions.Largeur
            },

            Images = d.Images
                .OrderBy(i => i.OrdreAffichage)
                .Select(i => new ImageMeubleDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    OrdreAffichage = i.OrdreAffichage,
                    EstPrincipale = i.EstPrincipale
                })
                .ToList(),

            CaracteristiquesTechniques = new CaracteristiquesTechniquesMeubleDto
            {
                CompositionType = d.CaracteristiquesTechniques.CompositionType,
                Caisson = d.CaracteristiquesTechniques.Caisson,
                Facade = d.CaracteristiquesTechniques.Facade
            },

            Couleurs = d.MeubleCouleurs
                .Select(cc => new CouleurMeubleDto
                {
                    Id = cc.Couleur.Id,
                    Nom = cc.Couleur.Nom,
                    CodeCouleur = cc.Couleur.CodeCouleur,
                    ImagePresentationUrl = cc.Couleur.ImagePresentationUrl
                })
                .ToList()
        };
    }

    public async Task<MeubleDto?> UpdateAsync(string id, UpdateMeubleDto dto)
    {
        // 1. Récupérer le meuble existant AVEC toutes ses relations (via le repository)
        var meuble = await _meubleRepository.GetByIdAsync(id);
        if (meuble == null)
            return null; // Le meuble n'existe pas -> le contrôleur renverra un 404

        // 2. Mettre à jour les propriétés simples de l'entité meuble
        //meuble.Categorie = dto.Categorie;
        meuble.Forme = dto.Forme;
        meuble.Nom = dto.Nom;
        meuble.PrixInitial = dto.PrixInitial;
        meuble.Reduction = dto.Reduction;
        meuble.Description = dto.Description;

        // 3. Mettre à jour les Dimensions (comme c'est un objet 1-to-1, on met à jour ses propriétés)
        if (meuble.Dimensions != null)
        {
            meuble.Dimensions.Hauteur = dto.Dimensions.Hauteur;
            meuble.Dimensions.Profondeur = dto.Dimensions.Profondeur;
            meuble.Dimensions.Largeur = dto.Dimensions.Largeur;
        }
        else
        {
            // Sécurité au cas où elles n'existeraient pas
            meuble.Dimensions = new DimensionsMeuble
            {
                Hauteur = dto.Dimensions.Hauteur,
                Profondeur = dto.Dimensions.Profondeur,
                Largeur = dto.Dimensions.Largeur
            };
        }

        // 4. Mettre à jour les Caractéristiques Techniques
        if (meuble.CaracteristiquesTechniques != null)
        {
            meuble.CaracteristiquesTechniques.CompositionType = dto.CaracteristiquesTechniques.CompositionType;
            meuble.CaracteristiquesTechniques.Caisson = dto.CaracteristiquesTechniques.Caisson;
            meuble.CaracteristiquesTechniques.Facade = dto.CaracteristiquesTechniques.Facade;
        }
        else
        {
            meuble.CaracteristiquesTechniques = new CaracteristiquesTechniquesMeuble
            {
                CompositionType = dto.CaracteristiquesTechniques.CompositionType,
                Caisson = dto.CaracteristiquesTechniques.Caisson,
                Facade = dto.CaracteristiquesTechniques.Facade
            };
        }

        // 5. Gérer les Couleurs (Relation Many-to-Many via meubleCouleurs)
        // La stratégie la plus propre en PUT : On vide la liste actuelle et on recrée les associations demandées
        meuble.MeubleCouleurs.Clear();
        foreach (var couleurId in dto.CouleurIds.Distinct())
        {
            meuble.MeubleCouleurs.Add(new MeubleCouleur
            {
                MeubleId = meuble.Id,
                CouleurId = couleurId
            });
        }

        // 6. Gérer les Images (Attention au stockage physique/cloud et aux images existantes)
        // Ici, on gère le cas où l'utilisateur envoie de nouvelles images ou garde les existantes.
        var updatedImages = new List<ImageMeuble>();

        foreach (var imageDto in dto.Images)
        {
            // CAS 1 : C'est une NOUVELLE image envoyée sous forme de fichier
            if (imageDto.FileStream != null && imageDto.FileStream.Length > 0)
            {
                var uploadResult = await _imageStorageService.UploadImageAsync(
                    imageDto.FileStream,
                    imageDto.FileName);

                updatedImages.Add(new ImageMeuble
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
                updatedImages.Add(new ImageMeuble
                {
                    Id = imageDto.Id.Value, // On garde l'ID existant
                    Url = imageDto.Url,     // On garde l'URL existante
                    OrdreAffichage = imageDto.OrdreAffichage,
                    EstPrincipale = imageDto.EstPrincipale
                });
            }
        }

        // On remplace l'ancienne liste par la nouvelle
        meuble.Images = updatedImages;

        // 7. Sauvegarder via le Repository
        await _meubleRepository.UpdateAsync(meuble);

        // 8. Re-récupérer proprement l'objet mis à jour pour garantir l'intégrité (exactement comme dans Create)
        var meubleMisAJour = await _meubleRepository.GetByIdAsync(meuble.Id);
        if (meubleMisAJour is null)
            throw new InvalidOperationException("Le meuble a été mis à jour, mais sa récupération a échoué.");

        // 9. Retourner le DTO formaté
        return MapToDto(meubleMisAJour);
    }
}
