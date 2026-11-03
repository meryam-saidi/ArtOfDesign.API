
using ArtOfDesign.Application.Interfaces;
using ArtOfDesign.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ArtOfDesign.Application.Interfaces.Cuisines;
using ArtOfDesign.Infrastructure.Repositories.Cuisines;
using ArtOfDesign.Application.Interfaces.Couleurs;
using ArtOfDesign.Infrastructure.Repositories.Couleurs;

namespace ArtOfDesign.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IImageStorageService>(provider =>
        {
            var cloudName = configuration["Cloudinary:CloudName"]
                ?? throw new InvalidOperationException("Cloudinary CloudName is missing.");

            var apiKey = configuration["Cloudinary:ApiKey"]
                ?? throw new InvalidOperationException("Cloudinary ApiKey is missing.");

            var apiSecret = configuration["Cloudinary:ApiSecret"]
                ?? throw new InvalidOperationException("Cloudinary ApiSecret is missing.");

            return new CloudinaryService(cloudName, apiKey, apiSecret);
        });

        services.AddScoped<ICuisineRepository, CuisineRepository>();
        services.AddScoped<ICouleurRepository, CouleurRepository>();

        return services;
    }
}
