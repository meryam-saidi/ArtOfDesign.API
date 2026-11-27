
namespace ArtOfDesign.Domain.Entities.Authentification;

public class Admin
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    public DateTime? DerniereConnexion { get; set; }
}
