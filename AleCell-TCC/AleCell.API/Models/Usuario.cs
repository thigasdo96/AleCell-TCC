using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AleCell.API.Models;

public class Usuario : IdentityUser
{
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    public DateTime? DataNascimento { get; set; }

    public string Foto { get; set; }
}
