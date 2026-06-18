using System.Diagnostics;
using AleCell.UI.DTOs;
using AleCell.UI.Models;
using AleCell.UI.Services.Interfaces;
using AleCell.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AleCell.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILojaService _lojaService;

    public HomeController(ILogger<HomeController> logger, ILojaService lojaService)
    {
        _logger = logger;
        _lojaService = lojaService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var categorias = await _lojaService.ObterCategoriasAsync();
            var produtosDestaque = await _lojaService.ObterProdutosDestaqueAsync();
            return View(new HomeVM { Categorias = categorias, ProdutosDestaque = produtosDestaque });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar página inicial");
            return View(new HomeVM());
        }
    }

    public async Task<IActionResult> Catalogo(int? categoriaId)
    {
        try
        {
            ViewBag.Categorias = await _lojaService.ObterCategoriasAsync();
            List<ProdutoDto> produtos;
            if (categoriaId.HasValue && categoriaId > 0)
            {
                produtos = await _lojaService.ObterProdutosPorCategoriaAsync(categoriaId.Value);
                ViewBag.CategoriaSelecionada = categoriaId.Value;
            }
            else
            {
                produtos = await _lojaService.ObterTodosProdutosAsync();
                ViewBag.CategoriaSelecionada = 0;
            }
            return View(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar catálogo");
            TempData["Erro"] = "Erro ao carregar produtos.";
            return View(new List<ProdutoDto>());
        }
    }

    public async Task<IActionResult> Produto(int id)
    {
        try
        {
            var produto = await _lojaService.ObterProdutoPorIdAsync(id);
            if (produto == null)
            {
                TempData["Erro"] = "Produto não encontrado.";
                return RedirectToAction("Catalogo");
            }
            return View(produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar produto {Id}", id);
            TempData["Erro"] = "Erro ao carregar produto.";
            return RedirectToAction("Catalogo");
        }
    }

    public IActionResult Conserto() => View();
    public IActionResult Mapa() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
