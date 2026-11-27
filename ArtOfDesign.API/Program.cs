using ArtOfDesign.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ArtOfDesign.Infrastructure;
using ArtOfDesign.Application.Interfaces.Cuisines;
using ArtOfDesign.Application.Services.Cuisines;
using ArtOfDesign.Application.Services.Couleurs;
using ArtOfDesign.Application.Interfaces.Couleurs;
using ArtOfDesign.Application.Interfaces.Dressings;
using ArtOfDesign.Application.Services.Dressings;
using ArtOfDesign.Application.Services.Meubles;
using ArtOfDesign.Application.Interfaces.Meubles;
using ArtOfDesign.Application.Interfaces.Projets;
using ArtOfDesign.Application.Services.Projets;
using ArtOfDesign.Application.Interfaces.Authentification;
using ArtOfDesign.Infrastructure.Services;
using ArtOfDesign.Domain.Entities.Authentification;
using Microsoft.AspNetCore.Authentication.Cookies;
using ArtOfDesign.Application.Services.Authentification;
using ArtOfDesign.Infrastructure.Repositories.Authentification;
using Microsoft.AspNetCore.Authorization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(Options =>
    Options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICuisineService, CuisineService>();

builder.Services.AddScoped<ICouleurService, CouleurService>();

builder.Services.AddScoped<IDressingService, DressingService>();

builder.Services.AddScoped<IMeubleService, MeubleService>();

builder.Services.AddScoped<IProjetService, ProjetService>();

builder.Services.AddSingleton<IPasswordService, PasswordService>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

// --- Authentification par cookie ---
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "aod_admin";
        options.Cookie.HttpOnly = true;                          // illisible par le JavaScript
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // envoyé seulement en HTTPS
        options.Cookie.SameSite = builder.Environment.IsDevelopment()
            ? SameSiteMode.None   // dev : front et API sur des ports/protocoles différents
            : SameSiteMode.Lax;   // prod : front et API sur le même domaine

        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true; // prolongé tant que l'admin est actif

        // IMPORTANT pour une API : renvoyer 401/403 au lieu d'une redirection vers /Account/Login
        options.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization(options =>
{
    // Toute route sans attribut [Authorize]/[AllowAnonymous] exige d'être connecté
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// --- CORS ---
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AstroFrontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()); // obligatoire pour que le cookie passe
});

//limiter les tentatives de connexion
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("login", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "inconnu",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,                  // 5 tentatives...
                Window = TimeSpan.FromMinutes(1), // ...par minute
                QueueLimit = 0
            }));
});

var app = builder.Build();

//Le seed au démarrage
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();
    var config = app.Configuration.GetSection("AdminSeed");

    var email = config["Email"]?.Trim().ToLowerInvariant();
    var password = config["Password"];

    if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password)
        && !await db.Admins.AnyAsync())
    {
        var admin = new Admin
        {
            Email = email,
            UserName = config["UserName"]
        };
        admin.PasswordHash = passwordService.Hash(admin, password);

        db.Admins.Add(admin);
        await db.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Les middlewares
app.UseCors("AstroFrontend"); // middleware 1. CORS

app.UseHttpsRedirection();
app.UseRateLimiter();

app.UseAuthentication(); // middleware 2. Qui es-tu ? (lit le cookie)

app.UseAuthorization(); // middleware 3. As-tu le droit ?

app.MapControllers();

app.Run();
