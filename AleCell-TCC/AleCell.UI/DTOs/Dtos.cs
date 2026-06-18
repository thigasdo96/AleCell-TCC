using System.Text.Json.Serialization;

namespace AleCell.UI.DTOs;

public class UserDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;
    [JsonPropertyName("dataNascimento")]
    public DateTime? DataNascimento { get; set; }
    [JsonPropertyName("foto")]
    public string Foto { get; set; } = string.Empty;
    [JsonPropertyName("perfil")]
    public string Perfil { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
    [JsonPropertyName("expiration")]
    public DateTime Expiration { get; set; }
    [JsonPropertyName("user")]
    public UserDto User { get; set; } = null!;
}

public class CategoriaDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;
    [JsonPropertyName("foto")]
    public string Foto { get; set; } = string.Empty;
    [JsonPropertyName("cor")]
    public string Cor { get; set; } = "rgba(0,255,0,1)";
}

public class ProdutoDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("categoriaId")]
    public int CategoriaId { get; set; }
    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;
    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = string.Empty;
    [JsonPropertyName("qtde")]
    public int Qtde { get; set; } = 0;
    [JsonPropertyName("valorCusto")]
    public decimal ValorCusto { get; set; } = 0;
    [JsonPropertyName("valorVenda")]
    public decimal ValorVenda { get; set; } = 0;
    [JsonPropertyName("destaque")]
    public bool Destaque { get; set; } = false;
    [JsonPropertyName("foto")]
    public string Foto { get; set; } = string.Empty;
    [JsonPropertyName("categoriaNome")]
    public string CategoriaNome { get; set; } = string.Empty;
}

public class CarrinhoItemDto
{
    public int     ProdutoId     { get; set; }
    public string  Nome          { get; set; } = string.Empty;
    public string  Foto          { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
    public int     Quantidade    { get; set; }
    public int     EstoqueMax    { get; set; }
    public decimal Subtotal      => ValorUnitario * Quantidade;
}
