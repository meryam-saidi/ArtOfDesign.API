using ArtOfDesign.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Tests.Helpers;

public class SlugHelperTests
{
    // ---------- Slugify ----------

    [Theory]
    [InlineData("Appartement Kram", "appartement-kram")]
    [InlineData("Villa  Sidi Bou Saïd — Étage 2", "villa-sidi-bou-said-etage-2")]
    [InlineData("  Cuisine   Moderne  ", "cuisine-moderne")]
    [InlineData("Dressing #1 (Chêne)", "dressing-1-chene")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    public void Slugify_DonneLeBonSlug(string entree, string attendu)
    {
        var resultat = SlugHelper.Slugify(entree);

        Assert.Equal(attendu, resultat);
    }

    [Fact]
    public void Slugify_NomEnArabe_DonneUnSlugVide()
    {
        var resultat = SlugHelper.Slugify("مطبخ عصري");

        Assert.Equal(string.Empty, resultat);
    }

    [Fact]
    public void Slugify_RespecteLaLongueurMax_EtNeFinitPasParUnTiret()
    {
        var nomTresLong = new string('a', 30) + " " + new string('b', 30);

        var resultat = SlugHelper.Slugify(nomTresLong, maxLength: 31);

        Assert.True(resultat.Length <= 31);
        Assert.False(resultat.EndsWith('-'));
    }

    // ---------- GenerateUniqueIdAsync ----------

    [Fact]
    public async Task GenerateUniqueId_SlugLibre_RetourneLeSlugTelQuel()
    {
        // Arrange : aucun Id n'existe encore
        Func<string, Task<bool>> existe = _ => Task.FromResult(false);

        // Act
        var id = await SlugHelper.GenerateUniqueIdAsync("Appartement Kram", existe);

        // Assert
        Assert.Equal("appartement-kram", id);
    }

    [Fact]
    public async Task GenerateUniqueId_SlugDejaPris_AjouteUnSuffixe()
    {
        // Arrange : "appartement-kram" et "appartement-kram-2" existent déjà
        var dejaPris = new HashSet<string> { "appartement-kram", "appartement-kram-2" };
        Func<string, Task<bool>> existe = id => Task.FromResult(dejaPris.Contains(id));

        // Act
        var id = await SlugHelper.GenerateUniqueIdAsync("Appartement Kram", existe);

        // Assert
        Assert.Equal("appartement-kram-3", id);
    }

    [Fact]
    public async Task GenerateUniqueId_NomSansLettresLatines_UtiliseLePrefixe()
    {
        Func<string, Task<bool>> existe = _ => Task.FromResult(false);

        var id = await SlugHelper.GenerateUniqueIdAsync("مطبخ", existe, "projet");

        Assert.Equal("projet", id);
    }
}
