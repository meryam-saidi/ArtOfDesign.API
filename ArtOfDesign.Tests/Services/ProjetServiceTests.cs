using ArtOfDesign.Application.DTOs.Images;
using ArtOfDesign.Application.DTOs.Projets.Update;
using ArtOfDesign.Application.Interfaces.Projets;
using ArtOfDesign.Application.Interfaces;
using ArtOfDesign.Application.Services.Projets;
using ArtOfDesign.Domain.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;
using ArtOfDesign.Domain.Entities.Projets;

namespace ArtOfDesign.Tests.Services;

public class ProjetServiceTests
{
    // Les faux objets (mocks) : ni vraie base, ni vrai Cloudinary
    private readonly IProjetRepository _repository = Substitute.For<IProjetRepository>();
    private readonly IImageStorageService _storage = Substitute.For<IImageStorageService>();
    private readonly ProjetService _service;

    public ProjetServiceTests()
    {
        _service = new ProjetService(_repository, _storage);

        // Par défaut, chaque upload "réussit" et renvoie un nouveau PublicId
        var compteur = 0;
        _storage.UploadImageAsync(Arg.Any<Stream>(), Arg.Any<string>())
            .Returns(_ =>
            {
                compteur++;
                return new ImageUploadResult
                {
                    Url = $"https://cdn/new-{compteur}.jpg",
                    PublicId = $"new-{compteur}"
                };
            });
    }

    // Un projet de test avec 2 images existantes
    private static Projet CreerProjet() => new()
    {
        Id = "appartement-kram",
        Nom = "Appartement Kram",
        Description = "Test",
        Images = new List<ImageProjet>
        {
            new() { Id = 1, Title = "Vue 1", Url = "https://cdn/old-1.jpg", PublicId = "old-1",
                    Est3D = true, OrdreAffichage = 1, EstPrincipale = true, ProjetId = "appartement-kram" },
            new() { Id = 2, Title = "Vue 2", Url = "https://cdn/old-2.jpg", PublicId = "old-2",
                    Est3D = false, OrdreAffichage = 2, EstPrincipale = false, ProjetId = "appartement-kram" }
        }
    };

    private static UpdateProjetImageDto Garder(int id, int ordre, bool principale = false) => new()
    {
        Id = id,
        OrdreAffichage = ordre,
        EstPrincipale = principale
    };

    private static UpdateProjetImageDto Nouvelle(int ordre) => new()
    {
        Id = null,
        FileStream = new MemoryStream(new byte[] { 1, 2, 3 }),
        FileName = $"nouvelle-{ordre}.jpg",
        Est3D = false,
        OrdreAffichage = ordre
    };

    // ---------- UpdateAsync ----------

    [Fact]
    public async Task Update_ProjetInexistant_RetourneNull()
    {
        // Arrange : le repository ne trouve rien
        _repository.GetByIdAsync("inconnu").Returns((Projet?)null);

        // Act
        var resultat = await _service.UpdateAsync("inconnu", new UpdateProjetDto { Nom = "X" });

        // Assert
        Assert.Null(resultat);
        await _repository.DidNotReceive().UpdateAsync(Arg.Any<Projet>());
    }

    [Fact]
    public async Task Update_ImageAbsenteDeLaListe_EstSupprimeeEnBaseEtSurCloudinary()
    {
        // Arrange : on garde seulement l'image 1
        var projet = CreerProjet();
        _repository.GetByIdAsync(projet.Id).Returns(projet);

        var dto = new UpdateProjetDto
        {
            Nom = "Appartement Kram",
            Images = new List<UpdateProjetImageDto> { Garder(1, 1, principale: true) }
        };

        // Act
        await _service.UpdateAsync(projet.Id, dto);

        // Assert
        Assert.Single(projet.Images);
        Assert.Equal(1, projet.Images.First().Id);
        await _storage.Received(1).DeleteImageAsync("old-2");
        await _storage.DidNotReceive().DeleteImageAsync("old-1");
    }

    [Fact]
    public async Task Update_IdImageInconnu_LeveArgumentException()
    {
        var projet = CreerProjet();
        _repository.GetByIdAsync(projet.Id).Returns(projet);

        var dto = new UpdateProjetDto
        {
            Nom = "Appartement Kram",
            Images = new List<UpdateProjetImageDto> { Garder(999, 1) }
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(projet.Id, dto));
        await _repository.DidNotReceive().UpdateAsync(Arg.Any<Projet>());
    }

    [Fact]
    public async Task Update_NouvelleImageSansFichier_LeveArgumentException()
    {
        var projet = CreerProjet();
        _repository.GetByIdAsync(projet.Id).Returns(projet);

        var dto = new UpdateProjetDto
        {
            Nom = "Appartement Kram",
            Images = new List<UpdateProjetImageDto>
            {
                Garder(1, 1),
                new() { Id = null, OrdreAffichage = 2 } // pas de fichier
            }
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(projet.Id, dto));
    }

    [Fact]
    public async Task Update_EchecBase_SupprimeLesNouveauxFichiers_EtGardeLesAnciens()
    {
        // Arrange : l'enregistrement en base plante
        var projet = CreerProjet();
        _repository.GetByIdAsync(projet.Id).Returns(projet);
        _repository.UpdateAsync(Arg.Any<Projet>()).ThrowsAsync(new Exception("Base indisponible"));

        var dto = new UpdateProjetDto
        {
            Nom = "Appartement Kram",
            Images = new List<UpdateProjetImageDto> { Garder(1, 1), Nouvelle(2) } // l'image 2 est retirée
        };

        // Act
        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(projet.Id, dto));

        // Assert : le fichier fraîchement uploadé est nettoyé...
        await _storage.Received(1).DeleteImageAsync("new-1");
        // ...mais l'ancienne image n'est PAS supprimée de Cloudinary (la base n'a pas changé)
        await _storage.DidNotReceive().DeleteImageAsync("old-2");
    }

    [Fact]
    public async Task Update_PlusieursImagesPrincipales_UneSeuleEstGardee()
    {
        var projet = CreerProjet();
        _repository.GetByIdAsync(projet.Id).Returns(projet);

        var dto = new UpdateProjetDto
        {
            Nom = "Appartement Kram",
            Images = new List<UpdateProjetImageDto>
            {
                Garder(1, 1, principale: true),
                Garder(2, 2, principale: true)
            }
        };

        await _service.UpdateAsync(projet.Id, dto);

        Assert.Single(projet.Images, i => i.EstPrincipale);
    }

    // ---------- DeleteAsync ----------

    [Fact]
    public async Task Delete_SupprimeEnBaseAvantCloudinary()
    {
        var projet = CreerProjet();
        _repository.GetByIdAsync(projet.Id).Returns(projet);

        var resultat = await _service.DeleteAsync(projet.Id);

        Assert.True(resultat);
        Received.InOrder(async () =>
        {
            await _repository.DeleteAsync(projet);
            await _storage.DeleteImageAsync("old-1");
            await _storage.DeleteImageAsync("old-2");
        });
    }

    [Fact]
    public async Task Delete_ProjetInexistant_RetourneFalse()
    {
        _repository.GetByIdAsync("inconnu").Returns((Projet?)null);

        var resultat = await _service.DeleteAsync("inconnu");

        Assert.False(resultat);
        await _storage.DidNotReceive().DeleteImageAsync(Arg.Any<string>());
    }
}
