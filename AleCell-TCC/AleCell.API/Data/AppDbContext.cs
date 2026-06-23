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
                Nome = "iPhone 13",
                Descricao = "Equipado com o chip A15 Bionic, tela Super Retina XDR de 6,1 polegadas e sistema de câmera dupla de 12 MP com modo Cinema.",
                Qtde = 10, ValorCusto = 2800m, ValorVenda = 3499m, Destaque = false,
                Foto = "/img/produtos/iphone13.png"
            },
            new Produto
            {
                Id = 2, CategoriaId = 1,
                Nome = "iPhone 13 Pro",
                Descricao = "Chip A15 Bionic, tela Super Retina XDR com ProMotion (120Hz) e sistema de câmera Pro de 12 MP (Teleobjetiva, Wide e Ultrawide).",
                Qtde = 5, ValorCusto = 3500m, ValorVenda = 4299m, Destaque = false,
                Foto = "/img/produtos/iphone13pro.png"
            },
            new Produto
            {
                Id = 3, CategoriaId = 1,
                Nome = "iPhone 13 Pro Max",
                Descricao = "A maior tela da linha 13 com 6,7 polegadas, bateria de longa duração, chip A15 Bionic e câmeras Pro de alto desempenho.",
                Qtde = 5, ValorCusto = 4000m, ValorVenda = 4899m, Destaque = false,
                Foto = "/img/produtos/iphone13promax.png"
            },
            new Produto
            {
                Id = 4, CategoriaId = 1,
                Nome = "iPhone 14",
                Descricao = "iPhone 14 com chip A15 Bionic, câmera dupla avançada e detecção de acidente. Bateria de longa duração.",
                Qtde = 8, ValorCusto = 2800m, ValorVenda = 3999m, Destaque = false,
                Foto = "/img/produtos/iphone14.png"
            },
            new Produto
            {
                Id = 5, CategoriaId = 1,
                Nome = "iPhone 14 Pro",
                Descricao = "Introduz a Dynamic Island, chip A16 Bionic, tela Sempre Ativa e câmera principal de 48 MP para detalhes incríveis.",
                Qtde = 4, ValorCusto = 4500m, ValorVenda = 5499m, Destaque = false,
                Foto = "/img/produtos/iphone14pro.png"
            },
            new Produto
            {
                Id = 6, CategoriaId = 1,
                Nome = "iPhone 14 Pro Max",
                Descricao = "Tela de 6,7 polegadas com Dynamic Island, chip A16 Bionic, câmera de 48 MP e a melhor autonomia de bateria da linha 14.",
                Qtde = 3, ValorCusto = 5000m, ValorVenda = 6199m, Destaque = false,
                Foto = "/img/produtos/iphone14promax.png"
            },
            new Produto
            {
                Id = 7, CategoriaId = 1,
                Nome = "iPhone 15",
                Descricao = "iPhone 15 com chip A16 Bionic, câmera principal de 48MP e porta USB-C. Design em alumínio resistente com vidro Ceramic Shield.",
                Qtde = 8, ValorCusto = 3500m, ValorVenda = 4999m, Destaque = true,
                Foto = "/img/produtos/iphone15.png"
            },
            new Produto
            {
                Id = 8, CategoriaId = 1,
                Nome = "iPhone 15 Pro",
                Descricao = "Estrutura em titânio aeroespacial, chip A17 Pro, botão de Ação personalizável e sistema de câmera Pro avançado.",
                Qtde = 6, ValorCusto = 5500m, ValorVenda = 6799m, Destaque = false,
                Foto = "/img/produtos/iphone15pro.png"
            },
            new Produto
            {
                Id = 9, CategoriaId = 1,
                Nome = "iPhone 15 Pro Max",
                Descricao = "O iPhone mais avançado da Apple. Chip A17 Pro, câmera de 48MP com zoom óptico 5x, tela Super Retina XDR de 6.7 polegadas e titânio grau aeroespacial.",
                Qtde = 5, ValorCusto = 7000m, ValorVenda = 8999m, Destaque = true,
                Foto = "/img/produtos/iphone15promax.png"
            },
            new Produto
            {
                Id = 10, CategoriaId = 1,
                Nome = "iPhone 16",
                Descricao = "Novo Controle da Câmera, chip A18 ultraeficiente, fotos espaciais para Apple Vision Pro e cores vibrantes.",
                Qtde = 10, ValorCusto = 5200m, ValorVenda = 6499m, Destaque = true,
                Foto = "/img/produtos/iphone16.png"
            },
            new Produto
            {
                Id = 11, CategoriaId = 1,
                Nome = "iPhone 16 Pro",
                Descricao = "Tela maior de 6,3 polegadas, chip A18 Pro, gravação de vídeo 4K Dolby Vision a 120 qps e estrutura em titânio.",
                Qtde = 5, ValorCusto = 6800m, ValorVenda = 8299m, Destaque = true,
                Foto = "/img/produtos/iphone16pro.png"
            },
            new Produto
            {
                Id = 12, CategoriaId = 1,
                Nome = "iPhone 16 Pro Max",
                Descricao = "A maior tela de iPhone já feita (6,9 polegadas), chip A18 Pro, Controle da Câmera e bateria de altíssima duração.",
                Qtde = 5, ValorCusto = 7800m, ValorVenda = 9499m, Destaque = true,
                Foto = "/img/produtos/iphone16promax.png"
            },
            new Produto
            {
                Id = 13, CategoriaId = 1,
                Nome = "iPhone 17",
                Descricao = "Rumores indicam tela de 120Hz em toda a linha, chip A19 e design ultrafino. (Modelo em pré-lançamento/estimado).",
                Qtde = 2, ValorCusto = 6000m, ValorVenda = 7299m, Destaque = false,
                Foto = "/img/produtos/iphone17.png"
            },
            new Produto
            {
                Id = 14, CategoriaId = 1,
                Nome = "iPhone 17 Pro",
                Descricao = "Expectativa de 12GB de RAM, chip A19 Pro e câmeras de 48MP em todos os sensores. (Modelo em pré-lançamento/estimado).",
                Qtde = 2, ValorCusto = 7500m, ValorVenda = 8999m, Destaque = false,
                Foto = "/img/produtos/iphone17pro.png"
            },
            new Produto
            {
                Id = 15, CategoriaId = 1,
                Nome = "iPhone 17 Pro Max",
                Descricao = "O topo de linha absoluto com Face ID sob a tela e zoom óptico aprimorado. (Modelo em pré-lançamento/estimado).",
                Qtde = 2, ValorCusto = 8500m, ValorVenda = 10499m, Destaque = false,
                Foto = "/img/produtos/iphone17promax.png"
            },
            new Produto
            {
                Id = 16, CategoriaId = 2,
                Nome = "Xiaomi 14 Ultra",
                Descricao = "Flagship da Xiaomi com câmeras Leica, Snapdragon 8 Gen 3, tela AMOLED 120Hz e carregamento ultra-rápido de 90W.",
                Qtde = 3, ValorCusto = 4500m, ValorVenda = 5999m, Destaque = true,
                Foto = "/img/produtos/xiaomi14ultra.png"
            },
            new Produto
            {
                Id = 17, CategoriaId = 2,
                Nome = "Xiaomi Redmi Note 13 Pro",
                Descricao = "Melhor custo-benefício com câmera de 200MP, tela AMOLED 120Hz e bateria de 5100mAh. Carregamento rápido 67W.",
                Qtde = 12, ValorCusto = 1200m, ValorVenda = 1899m, Destaque = true,
                Foto = "/img/produtos/redminote13pro.png"
            },
            new Produto
            {
                Id = 18, CategoriaId = 3,
                Nome = "Capa iPhone 15 Pro Max - MagSafe",
                Descricao = "Capa oficial Apple com suporte MagSafe, proteção premium e acabamento em silicone.",
                Qtde = 20, ValorCusto = 150m, ValorVenda = 299m, Destaque = false,
                Foto = "/img/produtos/capa-iphone-magsafe.png"
            },
        };
        builder.Entity<Produto>().HasData(produtos);
    }
}
