
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Cuisines;
using Microsoft.EntityFrameworkCore;
using ArtOfDesign.Domain.Entities.Catalogue;

namespace ArtOfDesign.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cuisine> Cuisines => Set<Cuisine>();
    public DbSet<DimensionsCuisine> DimensionsCuisines => Set<DimensionsCuisine>();
    public DbSet<ImageCuisine> ImagesCuisine => Set<ImageCuisine>();
    public DbSet<CaracteristiquesTechniquesCuisine> CaracteristiquesTechniquesCuisines
        => Set<CaracteristiquesTechniquesCuisine>();
    public DbSet<Couleur> Couleurs => Set<Couleur>();
    public DbSet<CuisineCouleur> CuisineCouleurs => Set<CuisineCouleur>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new Configurations.CuisineConfiguration());

        modelBuilder.ApplyConfiguration(
            new Configurations.CaracteristiquesTechniquesCuisineConfiguration());
        
        modelBuilder.ApplyConfiguration(
            new Configurations.CouleurConfiguration());

        modelBuilder.ApplyConfiguration(
            new Configurations.CuisineCouleurConfiguration());

    }
}
