using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleCell.API.Models;

public class Produto
{
    public int Id { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    public Categoria Categoria { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(3000)]
    public string Descricao { get; set; } = string.Empty;

    public int Qtde { get; set; } = 0;

    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorCusto { get; set; } = 0;

    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorVenda { get; set; } = 0;

    public bool Destaque { get; set; } = false;

    public string Foto { get; set; }
}
