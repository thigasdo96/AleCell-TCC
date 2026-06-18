using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AleCell.API.Data;
using AleCell.API.DTOs;
using AleCell.API.Models;
using AleCell.API.Services.Interfaces;

namespace AleCell.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IFileService _fileService;

    public CategoriasController(AppDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
    {
        var categorias = await _context.Categorias.ToListAsync();
        foreach (var c in categorias)
            if (!string.IsNullOrEmpty(c.Foto))
                c.Foto = _fileService.GetFileUrl(c.Foto);
        return categorias;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> GetCategoria(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();
        if (!string.IsNullOrEmpty(categoria.Foto))
            categoria.Foto = _fileService.GetFileUrl(categoria.Foto);
        return categoria;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<Categoria>> PostCategoria([FromForm] CategoriaCreateDto dto)
    {
        var categoria = new Categoria
        {
            Nome = dto.Nome,
            Cor = dto.Cor ?? "rgba(0,255,0,1)"
        };
        if (dto.Foto != null && dto.Foto.Length > 0)
            categoria.Foto = await _fileService.SaveFileAsync(dto.Foto, "img/categorias");

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        if (!string.IsNullOrEmpty(categoria.Foto))
            categoria.Foto = _fileService.GetFileUrl(categoria.Foto);

        return CreatedAtAction("GetCategoria", new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> PutCategoria(int id, [FromForm] CategoriaUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();

        categoria.Nome = dto.Nome;
        categoria.Cor = dto.Cor;

        if (dto.Foto != null && dto.Foto.Length > 0)
        {
            if (!string.IsNullOrEmpty(categoria.Foto))
                await _fileService.DeleteFileAsync(categoria.Foto);
            categoria.Foto = await _fileService.SaveFileAsync(dto.Foto, "img/categorias");
        }

        _context.Entry(categoria).State = EntityState.Modified;
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Categorias.Any(e => e.Id == id)) return NotFound();
            throw;
        }

        if (!string.IsNullOrEmpty(categoria.Foto))
            categoria.Foto = _fileService.GetFileUrl(categoria.Foto);

        return Ok(categoria);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteCategoria(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();
        if (!string.IsNullOrEmpty(categoria.Foto))
            await _fileService.DeleteFileAsync(categoria.Foto);
        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
