using ArtOfDesign.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ArtOfDesign.Infrastructure;
using ArtOfDesign.Application.Interfaces.Cuisines;
using ArtOfDesign.Application.Services.Cuisines;
using ArtOfDesign.Application.Services.Couleurs;
using ArtOfDesign.Application.Interfaces.Couleurs;
using ArtOfDesign.Application.Interfaces;
using ArtOfDesign.Infrastructure.Services;

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


builder.Services.AddCors(options =>
{
    options.AddPolicy("AstroFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4321",
                "https://art-of-design-meubles.vercel.app/"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AstroFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
