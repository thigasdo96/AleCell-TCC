using AleCell.UI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AleCell.UI.Controllers;

[Authorize]
public class PerfilController : Controller
{
    private readonly IAuthService  _authService;
    private readonly ILojaService  _lojaService;

    public PerfilController(IAuthService authService, ILojaService lojaService)
    {
        _authService = authService;
        _lojaService = lojaService;
    }

    public async System.Threading.Tasks.Task<IActionResult> Index()
    {
        // Carrega os 2 últimos produtos em destaque como "últimas compras"
        var destaques = await _lojaService.ObterProdutosDestaqueAsync();
        var ultimos2  = destaques.Take(2).ToList();

        ViewBag.Nome    = _authService.GetUserName();
        ViewBag.Email   = _authService.GetUserEmail();
        ViewBag.Foto    = _authService.GetUserFoto();
        ViewBag.Perfil  = _authService.GetUserPerfil();

        return View(ultimos2);
    }
}
