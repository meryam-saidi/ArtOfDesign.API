using ArtOfDesign.Domain.Entities.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.Interfaces.Authentification;

public interface IPasswordService
{
    string Hash(Admin admin, string password);
    bool Verify(Admin admin, string password, out bool needsRehash);
}
