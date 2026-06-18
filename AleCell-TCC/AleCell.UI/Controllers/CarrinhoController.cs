using System.Text.Json;
using AleCell.UI.DTOs;
using AleCell.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AleCell.UI.Controllers;

public class CarrinhoController : Controller
{
    private readonly ILojaService _lojaService;
    private const string ChaveCarrinho = "Carrinho";

    public CarrinhoController(ILojaService lojaService)
    {
        _lojaService = lojaService;
    }

    // GET /Carrinho
    public IActionResult Index()
    {
        var carrinho = ObterCarrinho();
        return View(carrinho);
    }

    // GET /Carrinho/Adicionar/5
    public async Task<IActionResult> Adicionar(int id)
    {
        try
        {
            var produto = await _lojaService.ObterProdutoPorIdAsync(id);
            if (produto == null)
            {
                TempData["Erro"] = "Produto não encontrado.";
                return RedirectToAction("Catalogo", "Home");
            }

            if (produto.Qtde <= 0)
            {
                TempData["Erro"] = $"{produto.Nome} está sem estoque.";
                return RedirectToAction("Catalogo", "Home");
            }

            var carrinho = ObterCarrinho();
            var item = carrinho.FirstOrDefault(c => c.ProdutoId == id);

            if (item != null)
            {
                if (item.Quantidade < produto.Qtde)
                    item.Quantidade++;
                else
                    TempData["Erro"] = "Quantidade máxima em estoque já adicionada.";
            }
            else
            {
                carrinho.Add(new CarrinhoItemDto
                {
                    ProdutoId     = produto.Id,
                    Nome          = produto.Nome,
                    Foto          = produto.Foto,
                    ValorUnitario = produto.ValorVenda,
                    Quantidade    = 1,
                    EstoqueMax    = produto.Qtde
                });
                TempData["Sucesso"] = $"{produto.Nome} adicionado ao carrinho!";
            }

            SalvarCarrinho(carrinho);
        }
        catch
        {
            TempData["Erro"] = "Erro ao adicionar produto. Tente novamente.";
        }

        return RedirectToAction("Index");
    }

    // GET /Carrinho/ComprarAgora/5  — adiciona e vai direto ao checkout
    public async Task<IActionResult> ComprarAgora(int id)
    {
        try
        {
            var produto = await _lojaService.ObterProdutoPorIdAsync(id);
            if (produto != null && produto.Qtde > 0)
            {
                var carrinho = ObterCarrinho();
                var item = carrinho.FirstOrDefault(c => c.ProdutoId == id);
                if (item == null)
                    carrinho.Add(new CarrinhoItemDto
                    {
                        ProdutoId     = produto.Id,
                        Nome          = produto.Nome,
                        Foto          = produto.Foto,
                        ValorUnitario = produto.ValorVenda,
                        Quantidade    = 1,
                        EstoqueMax    = produto.Qtde
                    });
                SalvarCarrinho(carrinho);
            }
        }
        catch { }

        return RedirectToAction("Checkout");
    }

    // POST /Carrinho/AtualizarQuantidade
    [HttpPost]
    public IActionResult AtualizarQuantidade(int produtoId, int quantidade)
    {
        var carrinho = ObterCarrinho();
        var item = carrinho.FirstOrDefault(c => c.ProdutoId == produtoId);

        if (item != null)
        {
            if (quantidade <= 0)
                carrinho.Remove(item);
            else
                item.Quantidade = Math.Min(quantidade, item.EstoqueMax);
        }

        SalvarCarrinho(carrinho);
        return RedirectToAction("Index");
    }

    // GET /Carrinho/Remover/5
    public IActionResult Remover(int id)
    {
        var carrinho = ObterCarrinho();
        var item = carrinho.FirstOrDefault(c => c.ProdutoId == id);
        if (item != null) carrinho.Remove(item);
        SalvarCarrinho(carrinho);
        TempData["Sucesso"] = "Item removido do carrinho.";
        return RedirectToAction("Index");
    }

    // GET /Carrinho/Limpar
    public IActionResult Limpar()
    {
        SalvarCarrinho(new List<CarrinhoItemDto>());
        TempData["Sucesso"] = "Carrinho limpo.";
        return RedirectToAction("Index");
    }

    // GET /Carrinho/Checkout
    public IActionResult Checkout()
    {
        var carrinho = ObterCarrinho();
        if (!carrinho.Any())
        {
            TempData["Erro"] = "Seu carrinho está vazio.";
            return RedirectToAction("Index");
        }
        return View(carrinho);
    }

    // POST /Carrinho/FinalizarPedido
    [HttpPost]
    public IActionResult FinalizarPedido(string nome, string email, string telefone, string endereco, int parcelas)
    {
        var carrinho = ObterCarrinho();
        if (!carrinho.Any())
        {
            TempData["Erro"] = "Seu carrinho está vazio.";
            return RedirectToAction("Index");
        }

        var numeroPedido = $"ALC{DateTime.Now:yyyyMMddHHmmss}";
        SalvarCarrinho(new List<CarrinhoItemDto>());

        TempData["Sucesso"]      = $"Pedido #{numeroPedido} realizado com sucesso!";
        TempData["NumeroPedido"] = numeroPedido;
        TempData["Parcelas"]     = parcelas.ToString();

        return RedirectToAction("PedidoConfirmado");
    }

    // GET /Carrinho/PedidoConfirmado
    public IActionResult PedidoConfirmado()
    {
        return View();
    }

    // ---- Helpers ----
    private List<CarrinhoItemDto> ObterCarrinho()
    {
        var json = HttpContext.Session.GetString(ChaveCarrinho);
        if (string.IsNullOrEmpty(json)) return new List<CarrinhoItemDto>();
        return JsonSerializer.Deserialize<List<CarrinhoItemDto>>(json) ?? new List<CarrinhoItemDto>();
    }

    private void SalvarCarrinho(List<CarrinhoItemDto> carrinho)
    {
        HttpContext.Session.SetString(ChaveCarrinho, JsonSerializer.Serialize(carrinho));
    }
}
