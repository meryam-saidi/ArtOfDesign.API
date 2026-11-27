using System.Security.Claims;
using ArtOfDesign.Application.DTOs.Authentification;
using ArtOfDesign.Application.Interfaces.Authentification;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ArtOfDesign.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [AllowAnonymous]
    [EnableRateLimiting("login")]
    [HttpPost("login")]
    public async Task<ActionResult<AdminDto>> Login([FromBody] LoginRequestDto request)
    {
        var admin = await _authService.ValidateCredentialsAsync(request.Email, request.Password);

        // Message volontairement vague : on ne dit pas si c'est l'email ou le mot de passe qui est faux
        if (admin is null)
            return Unauthorized("Email ou mot de passe incorrect.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new(ClaimTypes.Email, admin.Email)
        };
        if (!string.IsNullOrWhiteSpace(admin.UserName))
            claims.Add(new Claim(ClaimTypes.Name, admin.UserName));

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = true }); // le cookie survit à la fermeture du navigateur

        return Ok(admin);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<AdminDto>> Me()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(idClaim, out var id))
            return Unauthorized();

        // Vérifie que l'admin existe toujours en base
        var admin = await _authService.GetAdminAsync(id);
        return admin is null ? Unauthorized() : Ok(admin);
    }
}
