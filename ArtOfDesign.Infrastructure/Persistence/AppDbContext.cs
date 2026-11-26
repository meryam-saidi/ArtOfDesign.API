
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Cuisines;
using Microsoft.EntityFrameworkCore;
using ArtOfDesign.Domain.Entities.Catalogue;
using ArtOfDesign.Domain.Entities.Dressings;
using ArtOfDesign.Domain.Entities.Meubles;
using ArtOfDesign.Domain.Entities.Projets;

namespace ArtOfDesign.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Couleur> Couleurs => Set<Couleur>();
    

    public DbSet<Cuisine> Cuisines => Set<Cuisine>();
    public DbSet<DimensionsCuisine> DimensionsCuisines => Set<DimensionsCuisine>();
    public DbSet<ImageCuisine> ImagesCuisine => Set<ImageCuisine>();
    public DbSet<CaracteristiquesTechniquesCuisine> CaracteristiquesTechniquesCuisines
        => Set<CaracteristiquesTechniquesCuisine>(); 
    public DbSet<CuisineCouleur> CuisineCouleurs => Set<CuisineCouleur>();


    public DbSet<Dressing> Dressings => Set<Dressing>();
    public DbSet<DimensionsDressing> DimensionsDressings => Set<DimensionsDressing>(); 
    public DbSet<ImageDressing> ImagesDressing => Set<ImageDressing>();
    public DbSet<CaracteristiquesTechniquesDressing> CaracteristiquesTechniquesDressings 
        => Set<CaracteristiquesTechniquesDressing>();
    public DbSet<DressingCouleur> DressingCouleurs => Set<DressingCouleur>();


    public DbSet<Meuble> Meubles => Set<Meuble>();
    public DbSet<DimensionsMeuble> DimensionsMeubles => Set<DimensionsMeuble>();
    public DbSet<ImageMeuble> ImagesMeuble => Set<ImageMeuble>();
    public DbSet<CaracteristiquesTechniquesMeuble> CaracteristiquesTechniquesMeubles
        => Set<CaracteristiquesTechniquesMeuble>();
    public DbSet<MeubleCouleur> MeubleCouleurs => Set<MeubleCouleur>();


    public DbSet<Projet> Projets => Set<Projet>();
    public DbSet<ImageProjet> ImagesProjet => Set<ImageProjet>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.ApplyConfiguration(
            new Configurations.CouleurConfiguration());


        modelBuilder.ApplyConfiguration(new Configurations.Cuisines.CuisineConfiguration());
        modelBuilder.ApplyConfiguration(
            new Configurations.Cuisines.CaracteristiquesTechniquesCuisineConfiguration());
        modelBuilder.ApplyConfiguration(
            new Configurations.Cuisines.CuisineCouleurConfiguration());


        modelBuilder.ApplyConfiguration(new Configurations.Dressings.DressingConfiguration());
        modelBuilder.ApplyConfiguration(
            new Configurations.Dressings.CaracteristiquesTechniquesDressingConfiguration());
        modelBuilder.ApplyConfiguration(
            new Configurations.Dressings.DressingCouleurConfiguration());


        modelBuilder.ApplyConfiguration(new Configurations.Meubles.MeubleConfiguration());
        modelBuilder.ApplyConfiguration(
            new Configurations.Meubles.CaracteristiquesTechniquesMeubleConfiguration());
        modelBuilder.ApplyConfiguration(
            new Configurations.Meubles.MeubleCouleurConfiguration());


        modelBuilder.ApplyConfiguration(new Configurations.Projets.ProjetConfiguration());

    }
}
