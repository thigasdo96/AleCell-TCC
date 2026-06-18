using System.ComponentModel.DataAnnotations;

namespace AleCell.API.DTOs;

// ============ AUTH DTOs ============

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Senha { get; set; }

    [Required]
    public string Nome { get; set; }

    public DateTime? DataNascimento { get; set; }

    public IFormFile Foto { get; set; }
}

public class LoginDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Senha { get; set; }
}

public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Nome { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string Foto { get; set; }
    public string Perfil { get; set; }
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public UserDto User { get; set; } = null!;
}

// ============ CATEGORIA DTOs ============

public class CategoriaCreateDto
{
    [Required]
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(26)]
    public string Cor { get; set; }

    public IFormFile Foto { get; set; }
}

public class CategoriaUpdateDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(26)]
    public string Cor { get; set; }

    public IFormFile Foto { get; set; }
}

// ============ PRODUTO DTOs ============

public class ProdutoDto
{
    public int Id { get; set; }
    public int CategoriaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Qtde { get; set; } = 0;
    public decimal ValorCusto { get; set; } = 0;
    public decimal ValorVenda { get; set; } = 0;
    public bool Destaque { get; set; } = false;
    public string Foto { get; set; }
    public string CategoriaNome { get; set; }
}

public class ProdutoCreateDto
{
    [Required]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(3000)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue)]
    public int Qtde { get; set; } = 0;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ValorCusto { get; set; } = 0;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ValorVenda { get; set; } = 0;

    public bool Destaque { get; set; } = false;

    public IFormFile Foto { get; set; }
}

public class ProdutoUpdateDto
{
    public int Id { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(3000)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue)]
    public int Qtde { get; set; } = 0;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ValorCusto { get; set; } = 0;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ValorVenda { get; set; } = 0;

    public bool Destaque { get; set; } = false;

    public IFormFile Foto { get; set; }
}
