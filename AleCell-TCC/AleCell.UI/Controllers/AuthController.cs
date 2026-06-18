using AleCell.UI.Services.Interfaces;
using AleCell.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AleCell.UI.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Login()
    {
        if (_authService.IsAuthenticated())
            return RedirectBasedOnPerfil();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid) return View(loginVM);

        var (success, message) = await _authService.LoginAsync(loginVM);
        if (success)
            return RedirectBasedOnPerfil();

        ModelState.AddModelError(string.Empty, message);
        return View(loginVM);
    }

    public IActionResult Registro()
    {
        if (_authService.IsAuthenticated())
            return RedirectBasedOnPerfil();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(RegistroVM registroVM)
    {
        if (!ModelState.IsValid) return View(registroVM);

        var (success, message) = await _authService.RegisterAsync(registroVM);
        if (success)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError(string.Empty, message);
        return View(registroVM);
    }

    public IActionResult Logout()
    {
        _authService.Logout();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AcessoNegado()
    {
        return View();
    }

    private IActionResult RedirectBasedOnPerfil()
    {
        if (_authService.GetUserPerfil() == "Administrador")
            return RedirectToAction("Index", "Admin");
        return RedirectToAction("Index", "Home");
    }
}
