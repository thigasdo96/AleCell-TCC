using AleCell.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AleCell.API.Data;

public class AppDbContext : IdentityDbContext<Usuario>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedUsuarioPadrao(builder);
        SeedCategoriaPadrao(builder);
        SeedProdutoPadrao(builder);
    }

    private static void SeedUsuarioPadrao(ModelBuilder builder)
    {
        #region Populate Roles
        List<IdentityRole> roles =
        [
            new IdentityRole()
            {
                Id = "0b44ca04-f6b0-4a8f-a953-1f2330d30894",
                Name = "Administrador",
                NormalizedName = "ADMINISTRADOR"
            },
            new IdentityRole()
            {
                Id = "ddf093a6-6cb5-4ff7-9a64-83da34aee005",
                Name = "Cliente",
                NormalizedName = "CLIENTE"
            },
        ];
        builder.Entity<IdentityRole>().HasData(roles);
        #endregion

        #region Populate Usuário Admin
        List<Usuario> usuarios = [
            new Usuario()
            {
                Id = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
                Email = "admin@alecell.com.br",
                NormalizedEmail = "ADMIN@ALECELL.COM.BR",
                UserName = "admin@alecell.com.br",
                NormalizedUserName = "ADMIN@ALECELL.COM.BR",
                LockoutEnabled = true,
                EmailConfirmed = true,
                Nome = "Administrador AleCell",
                DataNascimento = new DateTime(1990, 1, 1),
                Foto = "/img/usuarios/avatar.png"
            }
        ];
        foreach (var user in usuarios)
        {
            PasswordHasher<Usuario> pass = new();
            user.PasswordHash = pass.HashPassword(user, "Admin@123");
        }
        builder.Entity<Usuario>().HasData(usuarios);
        #endregion

        #region Populate UserRole
        List<IdentityUserRole<string>> userRoles =
        [
            new IdentityUserRole<string>()
            {
                UserId = usuarios[0].Id,
                RoleId = roles[0].Id
            }
        ];
        builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        #endregion
    }

    private static void SeedCategoriaPadrao(ModelBuilder builder)
    {
        List<Categoria> categorias = new()
        {
            new Categoria { Id = 1, Nome = "Apple", Cor = "rgba(100,100,100,1)", Foto = "/img/categorias/apple.png" },
            new Categoria { Id = 2, Nome = "Xiaomi", Cor = "rgba(255,100,0,1)", Foto = "/img/categorias/xiaomi.png" },
            new Categoria { Id = 3, Nome = "Acessórios", Cor = "rgba(0,180,255,1)", Foto = "/img/categorias/acessorios.png" },
            new Categoria { Id = 4, Nome = "Conserto", Cor = "rgba(0,200,100,1)", Foto = "/img/categorias/conserto.png" },
        };
        builder.Entity<Categoria>().HasData(categorias);
    }

    private static void SeedProdutoPadrao(ModelBuilder builder)
    {
        List<Produto> produtos = new()
        {
            new Produto
            {
                Id = 1, CategoriaId = 1,
                Nome = "iPhone 15 Pro Max",
                Descricao = "O iPhone mais avançado da Apple. Chip A17 Pro, câmera de 48MP com zoom óptico 5x, tela Super Retina XDR de 6.7 polegadas e titânio grau aeroespacial.",
                Qtde = 5, ValorCusto = 7000m, ValorVenda = 8999m, Destaque = true,
                Foto = "/img/produtos/iphone15promax.png"
            },
            new Produto
            {
                Id = 2, CategoriaId = 1,
                Nome = "iPhone 15",
                Descricao = "iPhone 15 com chip A16 Bionic, câmera principal de 48MP e porta USB-C. Design em alumínio resistente com vidro Ceramic Shield.",
                Qtde = 8, ValorCusto = 3500m, ValorVenda = 4999m, Destaque = true,
                Foto = "/img/produtos/iphone15.png"
            },
            new Produto
            {
                Id = 3, CategoriaId = 1,
                Nome = "iPhone 14",
                Descricao = "iPhone 14 com chip A15 Bionic, câmera dupla avançada e detecção de acidente. Bateria de longa duração.",
                Qtde = 0, ValorCusto = 2800m, ValorVenda = 3999m, Destaque = false,
                Foto = "/img/produtos/iphone14.png"
            },
            new Produto
            {
                Id = 4, CategoriaId = 2,
                Nome = "Xiaomi 14 Ultra",
                Descricao = "Flagship da Xiaomi com câmeras Leica, Snapdragon 8 Gen 3, tela AMOLED 120Hz e carregamento ultra-rápido de 90W.",
                Qtde = 3, ValorCusto = 4500m, ValorVenda = 5999m, Destaque = true,
                Foto = "/img/produtos/xiaomi14ultra.png"
            },
            new Produto
            {
                Id = 5, CategoriaId = 2,
                Nome = "Xiaomi Redmi Note 13 Pro",
                Descricao = "Melhor custo-benefício com câmera de 200MP, tela AMOLED 120Hz e bateria de 5100mAh. Carregamento rápido 67W.",
                Qtde = 12, ValorCusto = 1200m, ValorVenda = 1899m, Destaque = true,
                Foto = "/img/produtos/redminote13pro.png"
            },
            new Produto
            {
                Id = 6, CategoriaId = 3,
                Nome = "Capa iPhone 15 Pro Max - MagSafe",
                Descricao = "Capa oficial Apple com suporte MagSafe, proteção premium e acabamento em silicone.",
                Qtde = 20, ValorCusto = 150m, ValorVenda = 299m, Destaque = false,
                Foto = "/img/produtos/capa-iphone-magsafe.png"
            },
        };
        builder.Entity<Produto>().HasData(produtos);
    }
}
