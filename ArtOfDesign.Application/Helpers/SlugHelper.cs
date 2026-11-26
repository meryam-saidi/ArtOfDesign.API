using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ArtOfDesign.Application.Helpers;

public static class SlugHelper
{
    public static string Slugify(string texte, int maxLength = 50)
    {
        if (string.IsNullOrWhiteSpace(texte))
            return string.Empty;

        // 1. Supprimer les accents : "é" → "e", "ç" → "c"
        var normalise = texte.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalise)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        var slug = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();

        // 2. Remplacer tout ce qui n'est pas a-z / 0-9 par "-"
        slug = Regex.Replace(slug, @"[^a-z0-9]+", "-");

        // 3. Supprimer les "-" en début et en fin
        slug = slug.Trim('-');

        // 4. Respecter la longueur max de la colonne
        if (slug.Length > maxLength)
            slug = slug[..maxLength].TrimEnd('-');

        return slug;
    }

    public static async Task<string> GenerateUniqueIdAsync(
    string nom,
    Func<string, Task<bool>> existsAsync,
    string prefixeParDefaut = "item")
    {
        var baseSlug = Slugify(nom, maxLength: 45);
        if (string.IsNullOrEmpty(baseSlug))
            baseSlug = prefixeParDefaut;

        var slug = baseSlug;
        var i = 2;
        while (await existsAsync(slug))
            slug = $"{baseSlug}-{i++}";

        return slug;
    }
}
