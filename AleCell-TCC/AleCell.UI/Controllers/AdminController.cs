using AleCell.UI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AleCell.UI.Controllers;

[Authorize(Roles = "Administrador")]
public class AdminController : Controller
{
    private readonly IProdutoService   _produtoService;
    private readonly ICategoriaService _categoriaService;

    public AdminController(IProdutoService produtoService, ICategoriaService categoriaService)
    {
        _produtoService   = produtoService;
        _categoriaService = categoriaService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Menu = "Dashboard";

        try
        {
            var produtos    = await _produtoService.ObterTodosAsync()    ?? new();
            var categorias  = await _categoriaService.ObterTodasAsync()  ?? new();

            ViewBag.TotalProdutos   = produtos.Count;
            ViewBag.TotalCategorias = categorias.Count;
            ViewBag.TotalDestaques  = produtos.Count(p => p.Destaque);
            ViewBag.TotalEstoque    = produtos.Sum(p => p.Qtde);

            // 5 primeiros para a tabela
            ViewBag.UltimosProdutos = produtos.Take(5).ToList();

            // Estoque crítico (≤ 5 unidades), ordenado pelo menor
            ViewBag.EstoqueCritico  = produtos
                .Where(p => p.Qtde <= 5)
                .OrderBy(p => p.Qtde)
                .Take(8)
                .ToList();
        }
        catch
        {
            ViewBag.TotalProdutos   = "—";
            ViewBag.TotalCategorias = "—";
            ViewBag.TotalDestaques  = "—";
            ViewBag.TotalEstoque    = "—";
            ViewBag.UltimosProdutos = new List<object>();
            ViewBag.EstoqueCritico  = new List<object>();
            ViewBag.ErroApi = true;
        }

        return View();
    }
}
