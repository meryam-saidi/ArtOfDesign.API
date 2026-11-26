using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.Interfaces;

public interface IExistsById
{
    Task<bool> ExistsAsync(string id);
}
