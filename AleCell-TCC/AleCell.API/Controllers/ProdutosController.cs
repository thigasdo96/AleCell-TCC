using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AleCell.API.Data;
using AleCell.API.Models;
using AleCell.API.Services.Interfaces;
using AleCell.API.DTOs;

namespace AleCell.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IFileService _fileService;

    public ProdutosController(AppDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos()
    {
        var produtos = await _context.Produtos.Include(p => p.Categoria).ToListAsync();
        return Ok(produtos.Select(MapToDto).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoDto>> GetProduto(int id)
    {
        var produto = await _context.Produtos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
        if (produto == null) return NotFound();
        return Ok(MapToDto(produto));
    }

    [HttpGet("categoria/{categoriaId}")]
    public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutosPorCategoria(int categoriaId)
    {
        var produtos = await _context.Produtos.Include(p => p.Categoria)
            .Where(p => p.CategoriaId == categoriaId).ToListAsync();
        return Ok(produtos.Select(MapToDto).ToList());
    }

    [HttpGet("destaque")]
    public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutosDestaque()
    {
        var produtos = await _context.Produtos.Include(p => p.Categoria)
            .Where(p => p.Destaque).ToListAsync();
        return Ok(produtos.Select(MapToDto).ToList());
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ProdutoDto>> PostProduto([FromForm] ProdutoCreateDto dto)
    {
        var produto = new Produto
        {
            CategoriaId = dto.CategoriaId,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Qtde = dto.Qtde,
            ValorCusto = dto.ValorCusto,
            ValorVenda = dto.ValorVenda,
            Destaque = dto.Destaque
        };
        if (dto.Foto != null && dto.Foto.Length > 0)
            produto.Foto = await _fileService.SaveFileAsync(dto.Foto, "img/produtos");

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        await _context.Entry(produto).Reference(p => p.Categoria).LoadAsync();
        return CreatedAtAction("GetProduto", new { id = produto.Id }, MapToDto(produto));
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> PutProduto(int id, [FromForm] ProdutoUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null) return NotFound();

        produto.CategoriaId = dto.CategoriaId;
        produto.Nome = dto.Nome;
        produto.Descricao = dto.Descricao;
        produto.Qtde = dto.Qtde;
        produto.ValorCusto = dto.ValorCusto;
        produto.ValorVenda = dto.ValorVenda;
        produto.Destaque = dto.Destaque;

        if (dto.Foto != null && dto.Foto.Length > 0)
        {
            if (!string.IsNullOrEmpty(produto.Foto))
                await _fileService.DeleteFileAsync(produto.Foto);
            produto.Foto = await _fileService.SaveFileAsync(dto.Foto, "img/produtos");
        }

        _context.Entry(produto).State = EntityState.Modified;
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Produtos.Any(e => e.Id == id)) return NotFound();
            throw;
        }
        return Ok(MapToDto(produto));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null) return NotFound();
        if (!string.IsNullOrEmpty(produto.Foto))
            await _fileService.DeleteFileAsync(produto.Foto);
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private ProdutoDto MapToDto(Produto produto) => new ProdutoDto
    {
        Id = produto.Id,
        CategoriaId = produto.CategoriaId,
        Nome = produto.Nome,
        Descricao = produto.Descricao,
        Qtde = produto.Qtde,
        ValorCusto = produto.ValorCusto,
        ValorVenda = produto.ValorVenda,
        Destaque = produto.Destaque,
        Foto = !string.IsNullOrEmpty(produto.Foto) ? _fileService.GetFileUrl(produto.Foto) : null,
        CategoriaNome = produto.Categoria?.Nome
    };
}
