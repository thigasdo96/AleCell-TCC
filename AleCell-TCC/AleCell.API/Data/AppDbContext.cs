using AleCell.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AleCell.API.Data;

public class AppDbContext : IdentityDbContext<Usuario>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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
        List<IdentityRole> roles = [
            new IdentityRole() { Id = "0b44ca04-f6b0-4a8f-a953-1f2330d30894", Name = "Administrador", NormalizedName = "ADMINISTRADOR" },
            new IdentityRole() { Id = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", Name = "Cliente", NormalizedName = "CLIENTE" }
        ];
        builder.Entity<IdentityRole>().HasData(roles);

        List<Usuario> usuarios = [
            new Usuario() {
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
        foreach (var user in usuarios) {
            PasswordHasher<Usuario> pass = new();
            user.PasswordHash = pass.HashPassword(user, "Admin@123");
        }
        builder.Entity<Usuario>().HasData(usuarios);

        List<IdentityUserRole<string>> userRoles = [ new IdentityUserRole<string>() { UserId = usuarios[0].Id, RoleId = roles[0].Id } ];
        builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
    }

    private static void SeedCategoriaPadrao(ModelBuilder builder)
    {
        List<Categoria> categorias = [
            new Categoria { Id = 1, Nome = "Apple", Cor = "rgba(100,100,100,1)", Foto = "/img/categorias/apple.png" },
            new Categoria { Id = 2, Nome = "Xiaomi", Cor = "rgba(255,100,0,1)", Foto = "/img/categorias/xiaomi.png" },
            new Categoria { Id = 3, Nome = "Acessórios", Cor = "rgba(0,180,255,1)", Foto = "/img/categorias/acessorios.png" }
        ];
        builder.Entity<Categoria>().HasData(categorias);
    }

       private static void SeedProdutoPadrao(ModelBuilder builder)
    {
        List<Produto> produtos = [
            // Apple (1-15) - Mantive os nomes padrão
            new Produto { Id = 1, CategoriaId = 1, Nome = "iPhone 13", Descricao = "Chip A15 Bionic, Super Retina XDR.", Qtde = 10, ValorCusto = 2800m, ValorVenda = 3499m, Foto = "/img/produtos/iphone13.png" },
            new Produto { Id = 2, CategoriaId = 1, Nome = "iPhone 13 Pro", Descricao = "ProMotion 120Hz, câmeras Pro.", Qtde = 5, ValorCusto = 3500m, ValorVenda = 4299m, Foto = "/img/produtos/iphone13pro.png" },
            new Produto { Id = 3, CategoriaId = 1, Nome = "iPhone 13 Pro Max", Descricao = "Tela 6,7 pol, bateria longa.", Qtde = 5, ValorCusto = 4000m, ValorVenda = 4899m, Foto = "/img/produtos/iphone13promax.png" },
            new Produto { Id = 4, CategoriaId = 1, Nome = "iPhone 14", Descricao = "Câmera dupla avançada, chip A15.", Qtde = 8, ValorCusto = 2800m, ValorVenda = 3999m, Foto = "/img/produtos/iphone14.png" },
            new Produto { Id = 5, CategoriaId = 1, Nome = "iPhone 14 Pro", Descricao = "Dynamic Island, chip A16.", Qtde = 4, ValorCusto = 4500m, ValorVenda = 5499m, Foto = "/img/produtos/iphone14pro.png" },
            new Produto { Id = 6, CategoriaId = 1, Nome = "iPhone 14 Pro Max", Descricao = "Dynamic Island, tela 6,7 pol.", Qtde = 3, ValorCusto = 5000m, ValorVenda = 6199m, Foto = "/img/produtos/iphone14promax.png" },
            new Produto { Id = 7, CategoriaId = 1, Nome = "iPhone 15", Descricao = "Câmera 48MP, USB-C, A16.", Qtde = 8, ValorCusto = 3500m, ValorVenda = 4999m, Destaque = true, Foto = "/img/produtos/iphone15.png" },
            new Produto { Id = 8, CategoriaId = 1, Nome = "iPhone 15 Pro", Descricao = "Titânio, chip A17 Pro.", Qtde = 6, ValorCusto = 5500m, ValorVenda = 6799m, Foto = "/img/produtos/iphone15pro.png" },
            new Produto { Id = 9, CategoriaId = 1, Nome = "iPhone 15 Pro Max", Descricao = "Zoom 5x, titânio, A17 Pro.", Qtde = 5, ValorCusto = 7000m, ValorVenda = 8999m, Destaque = true, Foto = "/img/produtos/iphone15promax.png" },
            new Produto { Id = 10, CategoriaId = 1, Nome = "iPhone 16", Descricao = "Controle da Câmera, chip A18.", Qtde = 10, ValorCusto = 5200m, ValorVenda = 6499m, Destaque = true, Foto = "/img/produtos/iphone16.png" },
            new Produto { Id = 11, CategoriaId = 1, Nome = "iPhone 16 Pro", Descricao = "Gravação 4K 120 qps, A18 Pro.", Qtde = 5, ValorCusto = 6800m, ValorVenda = 8299m, Destaque = true, Foto = "/img/produtos/iphone16pro.png" },
            new Produto { Id = 12, CategoriaId = 1, Nome = "iPhone 16 Pro Max", Descricao = "Tela 6,9 pol, bateria máxima.", Qtde = 5, ValorCusto = 7800m, ValorVenda = 9499m, Destaque = true, Foto = "/img/produtos/iphone16promax.png" },
            new Produto { Id = 13, CategoriaId = 1, Nome = "iPhone 17", Descricao = "Próxima geração (Rumores).", Qtde = 2, ValorCusto = 6000m, ValorVenda = 7299m, Foto = "/img/produtos/iphone17.png" },
            new Produto { Id = 14, CategoriaId = 1, Nome = "iPhone 17 Pro", Descricao = "Performance extrema (Rumores).", Qtde = 2, ValorCusto = 7500m, ValorVenda = 8999m, Foto = "/img/produtos/iphone17pro.png" },
            new Produto { Id = 15, CategoriaId = 1, Nome = "iPhone 17 Pro Max", Descricao = "O topo de linha (Rumores).", Qtde = 2, ValorCusto = 8500m, ValorVenda = 10499m, Foto = "/img/produtos/iphone17promax.png" },

            // Xiaomi (16-27) - AJUSTADO PARA MINÚSCULAS
            new Produto { Id = 16, CategoriaId = 2, Nome = "Xiaomi 14 Ultra", Descricao = "Câmeras Leica, Snapdragon 8 Gen 3.", Qtde = 3, ValorCusto = 4500m, ValorVenda = 5999m, Destaque = true, Foto = "/img/produtos/xiaomi14ultra.png" },
            new Produto { Id = 17, CategoriaId = 2, Nome = "Xiaomi 15 Pro", Descricao = "Leica quad, tela cerâmica branca.", Qtde = 4, ValorCusto = 5500m, ValorVenda = 7200m, Destaque = true, Foto = "/img/produtos/xiaomi15pro.png" },
            new Produto { Id = 18, CategoriaId = 2, Nome = "Xiaomi 13T Pro", Descricao = "Carregamento 120W, Dimensity 9200+.", Qtde = 6, ValorCusto = 3200m, ValorVenda = 4199m, Destaque = true, Foto = "/img/produtos/xiaomi13tpro.png" },
            new Produto { Id = 19, CategoriaId = 2, Nome = "Xiaomi 13T", Descricao = "Câmeras Leica, tela 144Hz.", Qtde = 8, ValorCusto = 2500m, ValorVenda = 3299m, Foto = "/img/produtos/xiaomi13t.png" },
            new Produto { Id = 20, CategoriaId = 2, Nome = "Xiaomi MIX 4", Descricao = "Câmera sob a tela, cerâmica.", Qtde = 2, ValorCusto = 4000m, ValorVenda = 5499m, Foto = "/img/produtos/xiaomimix4.png" },
            new Produto { Id = 21, CategoriaId = 2, Nome = "Redmi Note 13 Pro", Descricao = "Câmera 200MP, carga 67W.", Qtde = 12, ValorCusto = 1300m, ValorVenda = 1999m, Destaque = true, Foto = "/img/produtos/redminote13pro.png" },
            new Produto { Id = 22, CategoriaId = 2, Nome = "Redmi Note 13", Descricao = "Tela AMOLED, bateria longa.", Qtde = 15, ValorCusto = 900m, ValorVenda = 1499m, Foto = "/img/produtos/redminote13.png" },
            new Produto { Id = 23, CategoriaId = 2, Nome = "Redmi 12", Descricao = "Snapdragon, tela AMOLED.", Qtde = 10, ValorCusto = 800m, ValorVenda = 1199m, Foto = "/img/produtos/redmi12.png" },
            new Produto { Id = 24, CategoriaId = 2, Nome = "Redmi 13 Pro", Descricao = "Design moderno, câmera top.", Qtde = 8, ValorCusto = 1100m, ValorVenda = 1699m, Foto = "/img/produtos/redmi13pro.png" },
            new Produto { Id = 25, CategoriaId = 2, Nome = "Redmi 12C", Descricao = "Custo-benefício imbatível.", Qtde = 20, ValorCusto = 600m, ValorVenda = 899m, Foto = "/img/produtos/redmi12c.png" },
            new Produto { Id = 26, CategoriaId = 2, Nome = "POCO X6 Pro", Descricao = "Gamer, Dimensity 8300-Ultra.", Qtde = 5, ValorCusto = 2000m, ValorVenda = 2799m, Destaque = true, Foto = "/img/produtos/pocox6pro.png" },
            new Produto { Id = 27, CategoriaId = 2, Nome = "POCO F5", Descricao = "Snapdragon 7+ Gen 2, 120Hz.", Qtde = 4, ValorCusto = 1800m, ValorVenda = 2399m, Foto = "/img/produtos/pocof5.png" },

            // Acessórios (28-38)
            new Produto { Id = 28, CategoriaId = 3, Nome = "Capa iPhone 15 Pro Max", Descricao = "Silicone MagSafe.", Qtde = 50, ValorCusto = 150m, ValorVenda = 299m, Foto = "/img/produtos/capa-iphone-magsafe.png" },
            new Produto { Id = 29, CategoriaId = 3, Nome = "AirPods Pro 2", Descricao = "Cancelamento de ruído.", Qtde = 10, ValorCusto = 1200m, ValorVenda = 1899m, Destaque = true, Foto = "/img/produtos/airpodspro2.png" },
            new Produto { Id = 30, CategoriaId = 3, Nome = "Xiaomi Buds 5", Descricao = "Áudio Hi-Fi.", Qtde = 15, ValorCusto = 300m, ValorVenda = 549m, Foto = "/img/produtos/xiaomibuds5.png" },
            new Produto { Id = 31, CategoriaId = 3, Nome = "Galaxy Buds2 Pro", Descricao = "Áudio 24 bits.", Qtde = 12, ValorCusto = 800m, ValorVenda = 1199m, Foto = "/img/produtos/galaxybuds2pro.png" },
            new Produto { Id = 32, CategoriaId = 3, Nome = "Apple Watch Series 9", Descricao = "Chip S9 SiP.", Qtde = 5, ValorCusto = 2500m, ValorVenda = 3499m, Destaque = true, Foto = "/img/produtos/applewatchseries9.png" },
            new Produto { Id = 33, CategoriaId = 3, Nome = "Xiaomi Watch S3", Descricao = "Design clássico.", Qtde = 8, ValorCusto = 700m, ValorVenda = 1099m, Foto = "/img/produtos/xiaomiwatchs3.png" },
            new Produto { Id = 34, CategoriaId = 3, Nome = "Carregador Apple 20W", Descricao = "USB-C original.", Qtde = 40, ValorCusto = 100m, ValorVenda = 199m, Foto = "/img/produtos/apple20wcharger.png" },
            new Produto { Id = 35, CategoriaId = 3, Nome = "Carregador Xiaomi 120W", Descricao = "HyperCharge.", Qtde = 10, ValorCusto = 250m, ValorVenda = 449m, Destaque = true, Foto = "/img/produtos/xiaomi120wcharger.png" },
            new Produto { Id = 36, CategoriaId = 3, Nome = "Carregador Xiaomi 67W", Descricao = "Carga rápida.", Qtde = 20, ValorCusto = 150m, ValorVenda = 279m, Foto = "/img/produtos/xiaomi67wcharger.png" },
            new Produto { Id = 37, CategoriaId = 3, Nome = "Carregador MagSafe Apple", Descricao = "Sem fio original.", Qtde = 15, ValorCusto = 300m, ValorVenda = 549m, Foto = "/img/produtos/magsafecharger.png" },
            new Produto { Id = 38, CategoriaId = 3, Nome = "Carregador Samsung 25W", Descricao = "USB-C Rápido.", Qtde = 30, ValorCusto = 80m, ValorVenda = 149m, Foto = "/img/produtos/samsung25wcharger.png" }
        ];
        builder.Entity<Produto>().HasData(produtos);
    }

}
