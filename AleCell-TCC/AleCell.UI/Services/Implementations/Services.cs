using AleCell.UI.DTOs;
using AleCell.UI.Models;
using AleCell.UI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace AleCell.UI.Services.Implementations;

public class CategoriaService : BaseApiService, ICategoriaService
{
    public CategoriaService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, IHttpContextAccessor httpContextAccessor)
        : base(httpClient, apiSettings, httpContextAccessor) { }

    public async Task<List<CategoriaDto>> ObterTodasAsync() => await GetAsync<List<CategoriaDto>>("categorias");
    public async Task<CategoriaDto> ObterPorIdAsync(int id) => await GetAsync<CategoriaDto>($"categorias/{id}");

    public async Task<bool> CriarComFotoAsync(CategoriaDto categoria, IFormFile foto)
    {
        try
        {
            AddAuthHeader();
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(categoria.Nome), "Nome");
            formData.Add(new StringContent(categoria.Cor ?? "rgba(0,255,0,1)"), "Cor");
            if (foto != null)
            {
                var fc = new StreamContent(foto.OpenReadStream());
                fc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(foto.ContentType);
                formData.Add(fc, "Foto", foto.FileName);
            }
            var response = await _httpClient.PostAsync("categorias", formData);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> AtualizarComFotoAsync(int id, CategoriaDto categoria, IFormFile foto)
    {
        try
        {
            AddAuthHeader();
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(id.ToString()), "Id");
            formData.Add(new StringContent(categoria.Nome), "Nome");
            formData.Add(new StringContent(categoria.Cor ?? "rgba(0,255,0,1)"), "Cor");
            if (foto != null)
            {
                var fc = new StreamContent(foto.OpenReadStream());
                fc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(foto.ContentType);
                formData.Add(fc, "Foto", foto.FileName);
            }
            var response = await _httpClient.PutAsync($"categorias/{id}", formData);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> ExcluirAsync(int id) => await DeleteAsync($"categorias/{id}");
}

public class ProdutoService : BaseApiService, IProdutoService
{
    public ProdutoService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, IHttpContextAccessor httpContextAccessor)
        : base(httpClient, apiSettings, httpContextAccessor) { }

    public async Task<List<ProdutoDto>> ObterTodosAsync() => await GetAsync<List<ProdutoDto>>("produtos");
    public async Task<ProdutoDto> ObterPorIdAsync(int id) => await GetAsync<ProdutoDto>($"produtos/{id}");

    public async Task<bool> CriarComFotoAsync(ProdutoDto produto, IFormFile foto)
    {
        try
        {
            AddAuthHeader();
            var formData = BuildProdutoForm(produto);
            if (foto != null)
            {
                var fc = new StreamContent(foto.OpenReadStream());
                fc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(foto.ContentType);
                formData.Add(fc, "Foto", foto.FileName);
            }
            var response = await _httpClient.PostAsync("produtos", formData);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> AtualizarComFotoAsync(int id, ProdutoDto produto, IFormFile foto)
    {
        try
        {
            AddAuthHeader();
            var formData = BuildProdutoForm(produto);
            formData.Add(new StringContent(id.ToString()), "Id");
            if (foto != null)
            {
                var fc = new StreamContent(foto.OpenReadStream());
                fc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(foto.ContentType);
                formData.Add(fc, "Foto", foto.FileName);
            }
            var response = await _httpClient.PutAsync($"produtos/{id}", formData);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> ExcluirAsync(int id) => await DeleteAsync($"produtos/{id}");

    private MultipartFormDataContent BuildProdutoForm(ProdutoDto p)
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(p.CategoriaId.ToString()), "CategoriaId");
        formData.Add(new StringContent(p.Nome ?? ""), "Nome");
        formData.Add(new StringContent(p.Descricao ?? ""), "Descricao");
        formData.Add(new StringContent(p.Qtde.ToString()), "Qtde");
        formData.Add(new StringContent(p.ValorCusto.ToString(System.Globalization.CultureInfo.InvariantCulture)), "ValorCusto");
        formData.Add(new StringContent(p.ValorVenda.ToString(System.Globalization.CultureInfo.InvariantCulture)), "ValorVenda");
        formData.Add(new StringContent(p.Destaque.ToString().ToLowerInvariant()), "Destaque");
        return formData;
    }
}

public class LojaService : BaseApiService, ILojaService
{
    public LojaService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, IHttpContextAccessor httpContextAccessor)
        : base(httpClient, apiSettings, httpContextAccessor) { }

    public async Task<List<CategoriaDto>> ObterCategoriasAsync() => await GetAsync<List<CategoriaDto>>("categorias");
    public async Task<List<ProdutoDto>> ObterProdutosDestaqueAsync() => await GetAsync<List<ProdutoDto>>("produtos/destaque");
    public async Task<List<ProdutoDto>> ObterTodosProdutosAsync() => await GetAsync<List<ProdutoDto>>("produtos");
    public async Task<List<ProdutoDto>> ObterProdutosPorCategoriaAsync(int categoriaId) => await GetAsync<List<ProdutoDto>>($"produtos/categoria/{categoriaId}");
    public async Task<ProdutoDto> ObterProdutoPorIdAsync(int id) => await GetAsync<ProdutoDto>($"produtos/{id}");
}
