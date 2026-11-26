using ArtOfDesign.Domain.Entities.Projets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Domain.Entities;

public class Projet
{
    public string Id { get; set; } = string.Empty;
    public string Categorie { get; set; } = "Projets";
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<ImageProjet> Images { get; set; } = new();

}
