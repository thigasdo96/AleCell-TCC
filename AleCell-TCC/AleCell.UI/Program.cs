using AleCell.UI;
using AleCell.UI.Models;
using AleCell.UI.Services.Implementations;
using AleCell.UI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configurações
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddScoped<UserContextService>();

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// Sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Autenticação via Cookie de Sessão
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "SessionAuth";
    options.DefaultChallengeScheme = "SessionAuth";
    options.DefaultSignInScheme = "SessionAuth";
})
.AddCookie("SessionAuth", options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AcessoNegado";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", p => p.RequireRole("Administrador"));
    options.AddPolicy("Cliente", p => p.RequireRole("Cliente"));
});

builder.Services.AddHttpContextAccessor();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

// Serviços de API — AddHttpClient injeta HttpClient corretamente em cada serviço
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<ICategoriaService, CategoriaService>();
builder.Services.AddHttpClient<IProdutoService, ProdutoService>();
builder.Services.AddHttpClient<ILojaService, LojaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

// Middleware para restaurar sessão do usuário como ClaimsPrincipal
app.Use(async (context, next) =>
{
    var userContextService = context.RequestServices.GetRequiredService<UserContextService>();
    context.User = userContextService.CreateClaimsPrincipal();
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// Rota padrão — Home pública
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Rota /admin redireciona para o painel administrativo
app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" });

app.Run();
