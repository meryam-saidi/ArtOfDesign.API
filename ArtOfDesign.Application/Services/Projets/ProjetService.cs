using ArtOfDesign.Application.Interfaces;
using ArtOfDesign.Application.Interfaces.Projets;
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Application.DTOs.Projets;
using ArtOfDesign.Domain.Entities.Projets;
using ArtOfDesign.Application.DTOs.Projets.Create;
using ArtOfDesign.Application.DTOs.Projets.Update;
using ArtOfDesign.Application.Helpers;

namespace ArtOfDesign.Application.Services.Projets;

public class ProjetService : IProjetService
{
    private readonly IProjetRepository _projetRepository;
    private readonly IImageStorageService _imageStorageService;
    
    private const string CloudinaryFolder = "art-of-design/projets";

    public ProjetService(IProjetRepository projetRepository, IImageStorageService imageStorageService)
    {
        _projetRepository = projetRepository;
        _imageStorageService = imageStorageService;
    }

    // ---------- Lecture ----------

    public async Task<List<ProjetDto>> GetAllAsync()
    {
        var projets = await _projetRepository.GetAllAsync();
        return projets.Select(MapToDto).ToList();
    }

    public async Task<ProjetDto?> GetByIdAsync(string id)
    {
        var projet = await _projetRepository.GetByIdAsync(id);
        return projet is null ? null : MapToDto(projet);
    }

    // ---------- Création ----------

    public async Task<ProjetDto> CreateAsync(CreateProjetDto dto)
    {
        var uploadedPublicIds = new List<string>();

        try
        {
            var images = new List<ImageProjet>();

            for (var i = 0; i < dto.Images.Count; i++)
            {
                var imageDto = dto.Images[i];

                if (imageDto.FileStream == Stream.Null || string.IsNullOrWhiteSpace(imageDto.FileName))
                    throw new ArgumentException($"L'image n°{i + 1} n'a pas de fichier.");

                var upload = await _imageStorageService.UploadImageAsync(imageDto.FileStream, imageDto.FileName);
                uploadedPublicIds.Add(upload.PublicId);

                var ordre = imageDto.OrdreAffichage > 0 ? imageDto.OrdreAffichage : i + 1;

                images.Add(new ImageProjet
                {
                    Title = TitreOuDefaut(imageDto.Title, dto.Nom, ordre),
                    Url = upload.Url,
                    PublicId = upload.PublicId,
                    Est3D = imageDto.Est3D,
                    OrdreAffichage = ordre,
                    EstPrincipale = imageDto.EstPrincipale
                });
            }

            NormaliserImagePrincipale(images);

            var projet = new Projet
            {
                Id = await SlugHelper.GenerateUniqueIdAsync(dto.Nom, _projetRepository.ExistsAsync, "projet"),
                Nom = dto.Nom.Trim(),
                Description = dto.Description.Trim(),
                Images = images
            };

            await _projetRepository.AddAsync(projet);

            return MapToDto(projet);
        }
        catch
        {
            // Nettoyer les fichiers déjà envoyés pour ne pas laisser d'orphelins
            await SupprimerFichiersAsync(uploadedPublicIds);
            throw;
        }
    }

    // ---------- Mise à jour ----------

    public async Task<ProjetDto?> UpdateAsync(string id, UpdateProjetDto dto)
    {
        var projet = await _projetRepository.GetByIdAsync(id); // doit être tracké, avec Include(Images)
        if (projet is null)
            return null;

        var uploadedPublicIds = new List<string>();      // à supprimer si échec
        var publicIdsASupprimer = new List<string>();    // à supprimer après succès

        try
        {
            // 1. Vérifier que les Id envoyés appartiennent bien à ce projet
            var idsEnvoyes = dto.Images.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToList();
            var idsInconnus = idsEnvoyes.Except(projet.Images.Select(i => i.Id)).ToList();
            if (idsInconnus.Count > 0)
                throw new ArgumentException($"Images introuvables pour ce projet : {string.Join(", ", idsInconnus)}");

            // 2. Supprimer les images absentes de la liste
            var aRetirer = projet.Images.Where(i => !idsEnvoyes.Contains(i.Id)).ToList();
            foreach (var image in aRetirer)
            {
                if (!string.IsNullOrWhiteSpace(image.PublicId))
                    publicIdsASupprimer.Add(image.PublicId);
                projet.Images.Remove(image);
            }

            // 3. Mettre à jour les existantes / ajouter les nouvelles
            for (var i = 0; i < dto.Images.Count; i++)
            {
                var imageDto = dto.Images[i];
                var ordre = imageDto.OrdreAffichage > 0 ? imageDto.OrdreAffichage : i + 1;
                var aUnFichier = imageDto.FileStream is not null && !string.IsNullOrWhiteSpace(imageDto.FileName);

                if (imageDto.Id.HasValue)
                {
                    var image = projet.Images.First(img => img.Id == imageDto.Id.Value);

                    if (aUnFichier)
                    {
                        var upload = await _imageStorageService.UploadImageAsync(imageDto.FileStream!, imageDto.FileName!);
                        uploadedPublicIds.Add(upload.PublicId);

                        if (!string.IsNullOrWhiteSpace(image.PublicId))
                            publicIdsASupprimer.Add(image.PublicId);

                        image.Url = upload.Url;
                        image.PublicId = upload.PublicId;
                    }

                    if (imageDto.Title is not null)
                        image.Title = TitreOuDefaut(imageDto.Title, dto.Nom, ordre);
                    if (imageDto.Est3D.HasValue)
                        image.Est3D = imageDto.Est3D.Value;

                    image.OrdreAffichage = ordre;
                    image.EstPrincipale = imageDto.EstPrincipale;
                }
                else
                {
                    if (!aUnFichier)
                        throw new ArgumentException($"La nouvelle image n°{i + 1} n'a pas de fichier.");

                    var upload = await _imageStorageService.UploadImageAsync(imageDto.FileStream!, imageDto.FileName!);
                    uploadedPublicIds.Add(upload.PublicId);

                    projet.Images.Add(new ImageProjet
                    {
                        Title = TitreOuDefaut(imageDto.Title, dto.Nom, ordre),
                        Url = upload.Url,
                        PublicId = upload.PublicId,
                        Est3D = imageDto.Est3D ?? false,
                        OrdreAffichage = ordre,
                        EstPrincipale = imageDto.EstPrincipale
                    });
                }
            }

            // 4. Champs du projet (l'Id/slug ne change pas)
            projet.Nom = dto.Nom.Trim();
            projet.Description = dto.Description.Trim();

            NormaliserImagePrincipale(projet.Images);

            await _projetRepository.UpdateAsync(projet);
        }
        catch
        {
            await SupprimerFichiersAsync(uploadedPublicIds);
            throw;
        }

        // 5. Base OK → on peut supprimer les anciens fichiers
        await SupprimerFichiersAsync(publicIdsASupprimer);

        return MapToDto(projet);
    }

    // ---------- Suppression ----------

    public async Task<bool> DeleteAsync(string id)
    {
        var projet = await _projetRepository.GetByIdAsync(id);
        if (projet is null)
            return false;

        var publicIds = projet.Images
            .Where(i => !string.IsNullOrWhiteSpace(i.PublicId))
            .Select(i => i.PublicId)
            .ToList();

        // Base d'abord (la cascade supprime les ImagesProjet), Cloudinary ensuite
        await _projetRepository.DeleteAsync(projet);
        await SupprimerFichiersAsync(publicIds);

        return true;
    }

    public async Task<bool> DeleteImageAsync(string projetId, int imageId)
    {
        var projet = await _projetRepository.GetByIdAsync(projetId);
        if (projet is null)
            return false;

        var image = projet.Images.FirstOrDefault(i => i.Id == imageId);
        if (image is null)
            return false;

        projet.Images.Remove(image);
        NormaliserImagePrincipale(projet.Images);

        await _projetRepository.DeleteImageAsync(image);

        if (!string.IsNullOrWhiteSpace(image.PublicId))
            await SupprimerFichiersAsync(new[] { image.PublicId });

        return true;
    }

    // ---------- Helpers ----------

    private static ProjetDto MapToDto(Projet p) => new()
    {
        Id = p.Id,
        Categorie = p.Categorie,
        Nom = p.Nom,
        Description = p.Description,
        Images = p.Images
            .OrderBy(i => i.OrdreAffichage)
            .Select(i => new ImageProjetDto
            {
                Id = i.Id,
                Title = i.Title,
                Url = i.Url,
                Est3D = i.Est3D,
                OrdreAffichage = i.OrdreAffichage,
                EstPrincipale = i.EstPrincipale
            })
            .ToList()
    };

    private static string TitreOuDefaut(string? titre, string nomProjet, int ordre)
    => string.IsNullOrWhiteSpace(titre) ? $"{nomProjet} — Vue {ordre}" : titre.Trim();

    /// Garantit exactement une image principale (la première marquée, sinon la première par ordre).
    private static void NormaliserImagePrincipale(ICollection<ImageProjet> images)
    {
        if (images.Count == 0) return;

        var principale = images.Where(i => i.EstPrincipale).OrderBy(i => i.OrdreAffichage).FirstOrDefault()
                         ?? images.OrderBy(i => i.OrdreAffichage).First();

        foreach (var img in images)
            img.EstPrincipale = ReferenceEquals(img, principale);
    }

    /// Suppression "best effort" : un échec Cloudinary ne doit pas faire échouer l'opération.
    private async Task SupprimerFichiersAsync(IEnumerable<string> publicIds)
    {
        foreach (var publicId in publicIds)
        {
            try { await _imageStorageService.DeleteImageAsync(publicId); }
            catch { /* idéalement : logger via ILogger<ProjetService> */ }
        }
    }
}
