using AutoMapper;
using AleCell.UI.DTOs;
using AleCell.UI.Services.Interfaces;
using AleCell.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AleCell.UI.Controllers;

[Authorize(Roles = "Administrador")]
public class CategoriasController : Controller
{
    private readonly ICategoriaService _categoriaService;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ICategoriaService categoriaService, IMapper mapper, ILogger<CategoriasController> logger)
    {
        _categoriaService = categoriaService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Menu = "Categorias";
        try
        {
            var categorias = await _categoriaService.ObterTodasAsync();
            return View(_mapper.Map<List<CategoriaVM>>(categorias));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar categorias");
            TempData["Erro"] = "Erro ao carregar categorias.";
            return View(new List<CategoriaVM>());
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        ViewBag.Menu = "Categorias";
        var categoria = await _categoriaService.ObterPorIdAsync(id);
        if (categoria == null) return RedirectToAction(nameof(Index));
        return View(_mapper.Map<CategoriaVM>(categoria));
    }

    public IActionResult Create()
    {
        ViewBag.Menu = "Categorias";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoriaVM vm)
    {
        if (!ModelState.IsValid) { ViewBag.Menu = "Categorias"; return View(vm); }
        var dto = _mapper.Map<CategoriaDto>(vm);
        var ok = await _categoriaService.CriarComFotoAsync(dto, vm.Foto);
        if (ok) { TempData["Sucesso"] = "Categoria criada!"; return RedirectToAction(nameof(Index)); }
        ModelState.AddModelError("", "Erro ao criar categoria.");
        ViewBag.Menu = "Categorias";
        return View(vm);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.Menu = "Categorias";
        var categoria = await _categoriaService.ObterPorIdAsync(id);
        if (categoria == null) return RedirectToAction(nameof(Index));
        return View(_mapper.Map<CategoriaVM>(categoria));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoriaVM vm)
    {
        if (!ModelState.IsValid) { ViewBag.Menu = "Categorias"; return View(vm); }
        var dto = _mapper.Map<CategoriaDto>(vm);
        var ok = await _categoriaService.AtualizarComFotoAsync(id, dto, vm.Foto);
        if (ok) { TempData["Sucesso"] = "Categoria atualizada!"; return RedirectToAction(nameof(Index)); }
        ModelState.AddModelError("", "Erro ao atualizar.");
        ViewBag.Menu = "Categorias";
        return View(vm);
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewBag.Menu = "Categorias";
        var categoria = await _categoriaService.ObterPorIdAsync(id);
        if (categoria == null) return RedirectToAction(nameof(Index));
        return View(_mapper.Map<CategoriaVM>(categoria));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ok = await _categoriaService.ExcluirAsync(id);
        TempData[ok ? "Sucesso" : "Erro"] = ok ? "Categoria excluída!" : "Erro ao excluir.";
        return RedirectToAction(nameof(Index));
    }
}
