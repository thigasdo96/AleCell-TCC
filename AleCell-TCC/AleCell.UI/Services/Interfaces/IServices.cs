using AleCell.UI.DTOs;
using AleCell.UI.ViewModels;

namespace AleCell.UI.Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string Message)> LoginAsync(LoginVM loginVM);
    Task<(bool Success, string Message)> RegisterAsync(RegistroVM registroVM);
    void Logout();
    bool IsAuthenticated();
    string GetUserToken();
    string GetUserName();
    string GetUserEmail();
    string GetUserFoto();
    string GetUserPerfil();
}

public interface ICategoriaService
{
    Task<List<CategoriaDto>> ObterTodasAsync();
    Task<CategoriaDto> ObterPorIdAsync(int id);
    Task<bool> CriarComFotoAsync(CategoriaDto categoria, IFormFile foto);
    Task<bool> AtualizarComFotoAsync(int id, CategoriaDto categoria, IFormFile foto);
    Task<bool> ExcluirAsync(int id);
}

public interface IProdutoService
{
    Task<List<ProdutoDto>> ObterTodosAsync();
    Task<ProdutoDto> ObterPorIdAsync(int id);
    Task<bool> CriarComFotoAsync(ProdutoDto produto, IFormFile foto);
    Task<bool> AtualizarComFotoAsync(int id, ProdutoDto produto, IFormFile foto);
    Task<bool> ExcluirAsync(int id);
}

public interface ILojaService
{
    Task<List<CategoriaDto>> ObterCategoriasAsync();
    Task<List<ProdutoDto>> ObterProdutosDestaqueAsync();
    Task<List<ProdutoDto>> ObterTodosProdutosAsync();
    Task<List<ProdutoDto>> ObterProdutosPorCategoriaAsync(int categoriaId);
    Task<ProdutoDto> ObterProdutoPorIdAsync(int id);
}
