using AutoMapper;
using AleCell.UI.DTOs;
using AleCell.UI.Services.Interfaces;
using AleCell.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AleCell.UI.Controllers;

[Authorize(Roles = "Administrador")]
public class ProdutosController : Controller
{
    private readonly IProdutoService _produtoService;
    private readonly ICategoriaService _categoriaService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(IProdutoService produtoService, ICategoriaService categoriaService, IMapper mapper, ILogger<ProdutosController> logger)
    {
        _produtoService = produtoService;
        _categoriaService = categoriaService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Menu = "Produtos";
        try
        {
            var produtos = await _produtoService.ObterTodosAsync();
            return View(_mapper.Map<List<ProdutoVM>>(produtos));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar produtos");
            TempData["Erro"] = "Erro ao carregar produtos.";
            return View(new List<ProdutoVM>());
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        ViewBag.Menu = "Produtos";
        var produto = await _produtoService.ObterPorIdAsync(id);
        if (produto == null) return RedirectToAction(nameof(Index));
        return View(_mapper.Map<ProdutoVM>(produto));
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Menu = "Produtos";
        await CarregarCategoriasViewBag();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProdutoVM vm)
    {
        if (!ModelState.IsValid) { await CarregarCategoriasViewBag(); ViewBag.Menu = "Produtos"; return View(vm); }
        var dto = new ProdutoDto
        {
            CategoriaId = vm.CategoriaId, Nome = vm.Nome, Descricao = vm.Descricao,
            Qtde = vm.Qtde, ValorCusto = vm.ValorCusto, ValorVenda = vm.ValorVenda, Destaque = vm.Destaque
        };
        var ok = await _produtoService.CriarComFotoAsync(dto, vm.Foto);
        if (ok) { TempData["Sucesso"] = "Produto criado!"; return RedirectToAction(nameof(Index)); }
        ModelState.AddModelError("", "Erro ao criar produto.");
        await CarregarCategoriasViewBag();
        ViewBag.Menu = "Produtos";
        return View(vm);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.Menu = "Produtos";
        var produto = await _produtoService.ObterPorIdAsync(id);
        if (produto == null) return RedirectToAction(nameof(Index));
        await CarregarCategoriasViewBag();
        return View(_mapper.Map<ProdutoVM>(produto));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProdutoVM vm)
    {
        if (!ModelState.IsValid) { await CarregarCategoriasViewBag(); ViewBag.Menu = "Produtos"; return View(vm); }
        var dto = new ProdutoDto
        {
            Id = vm.Id, CategoriaId = vm.CategoriaId, Nome = vm.Nome, Descricao = vm.Descricao,
            Qtde = vm.Qtde, ValorCusto = vm.ValorCusto, ValorVenda = vm.ValorVenda, Destaque = vm.Destaque
        };
        var ok = await _produtoService.AtualizarComFotoAsync(id, dto, vm.Foto);
        if (ok) { TempData["Sucesso"] = "Produto atualizado!"; return RedirectToAction(nameof(Index)); }
        ModelState.AddModelError("", "Erro ao atualizar.");
        await CarregarCategoriasViewBag();
        ViewBag.Menu = "Produtos";
        return View(vm);
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewBag.Menu = "Produtos";
        var produto = await _produtoService.ObterPorIdAsync(id);
        if (produto == null) return RedirectToAction(nameof(Index));
        return View(_mapper.Map<ProdutoVM>(produto));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ok = await _produtoService.ExcluirAsync(id);
        TempData[ok ? "Sucesso" : "Erro"] = ok ? "Produto excluído!" : "Erro ao excluir.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CarregarCategoriasViewBag()
    {
        try { ViewBag.Categorias = await _categoriaService.ObterTodasAsync(); }
        catch { ViewBag.Categorias = new List<CategoriaDto>(); }
    }
}
