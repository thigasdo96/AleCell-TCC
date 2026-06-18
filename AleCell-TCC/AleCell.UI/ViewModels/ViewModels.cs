using System.ComponentModel.DataAnnotations;
using AleCell.UI.DTOs;

namespace AleCell.UI.ViewModels;

public class LoginVM
{
    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
    public string Email { get; set; }

    [Display(Name = "Senha")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "A Senha é obrigatória")]
    public string Senha { get; set; }
}

public class RegistroVM
{
    [Display(Name = "Nome Completo")]
    [Required(ErrorMessage = "O Nome é obrigatório")]
    [StringLength(100)]
    public string Nome { get; set; }

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
    public string Email { get; set; }

    [Display(Name = "Senha")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "A Senha é obrigatória")]
    [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
    public string Senha { get; set; }

    [Display(Name = "Data de Nascimento")]
    [DataType(DataType.Date)]
    public DateTime? DataNascimento { get; set; }

    public IFormFile Foto { get; set; }
}

public class CategoriaVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Foto")]
    public IFormFile Foto { get; set; }

    public string FotoUrl { get; set; }

    [StringLength(26)]
    [Display(Name = "Cor (rgba ou hex)")]
    public string Cor { get; set; } = "rgba(0,255,0,1)";
}

public class ProdutoVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória")]
    [Display(Name = "Categoria")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    [Display(Name = "Nome do Produto")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(3000)]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Quantidade inválida")]
    [Display(Name = "Quantidade em Estoque")]
    public int Qtde { get; set; } = 0;

    [Required]
    [Range(0, double.MaxValue)]
    [Display(Name = "Valor de Custo (R$)")]
    public decimal ValorCusto { get; set; } = 0;

    [Required]
    [Range(0, double.MaxValue)]
    [Display(Name = "Valor de Venda (R$)")]
    public decimal ValorVenda { get; set; } = 0;

    [Display(Name = "Produto em Destaque")]
    public bool Destaque { get; set; } = false;

    [Display(Name = "Foto do Produto")]
    public IFormFile Foto { get; set; }

    public string FotoUrl { get; set; }
    public string CategoriaNome { get; set; }
}

public class HomeVM
{
    public List<CategoriaDto> Categorias { get; set; } = new();
    public List<ProdutoDto> ProdutosDestaque { get; set; } = new();
}
