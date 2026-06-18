using System.Text.Json;
using AleCell.UI.DTOs;
using AleCell.UI.Models;
using AleCell.UI.Services.Interfaces;
using AleCell.UI.ViewModels;
using Microsoft.Extensions.Options;

namespace AleCell.UI.Services.Implementations;

public class AuthService : BaseApiService, IAuthService
{
    public AuthService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, IHttpContextAccessor httpContextAccessor)
        : base(httpClient, apiSettings, httpContextAccessor) { }

    public async Task<(bool Success, string Message)> LoginAsync(LoginVM loginVM)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            var body = new { Email = loginVM.Email, Senha = loginVM.Senha };
            var content = new StringContent(JsonSerializer.Serialize(body), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("auth/login", content);

            // Ler o conteúdo UMA única vez
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var auth = JsonSerializer.Deserialize<AuthResponseDto>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (auth != null && !string.IsNullOrEmpty(auth.Token))
                {
                    SalvarSessao(auth);
                    return (true, "Login realizado com sucesso!");
                }
            }

            var err = TryParseError(responseBody);
            return (false, err ?? "Usuário e/ou senha inválidos.");
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegistroVM registroVM)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(registroVM.Nome ?? ""), "Nome");
            formData.Add(new StringContent(registroVM.Email ?? ""), "Email");
            formData.Add(new StringContent(registroVM.Senha ?? ""), "Senha");
            if (registroVM.DataNascimento.HasValue)
                formData.Add(new StringContent(registroVM.DataNascimento.Value.ToString("yyyy-MM-dd")), "DataNascimento");
            if (registroVM.Foto != null && registroVM.Foto.Length > 0)
            {
                var fc = new StreamContent(registroVM.Foto.OpenReadStream());
                fc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(registroVM.Foto.ContentType);
                formData.Add(fc, "Foto", registroVM.Foto.FileName);
            }

            var response = await _httpClient.PostAsync("auth/register", formData);

            // Ler o conteúdo UMA única vez
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var auth = JsonSerializer.Deserialize<AuthResponseDto>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (auth != null && !string.IsNullOrEmpty(auth.Token))
                {
                    SalvarSessao(auth);
                    return (true, "Cadastro realizado com sucesso!");
                }
            }

            return (false, TryParseError(responseBody) ?? "Falha no cadastro.");
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    private void SalvarSessao(AuthResponseDto auth)
    {
        _httpContextAccessor.HttpContext?.Session.SetString("JWTToken", auth.Token);
        _httpContextAccessor.HttpContext?.Session.SetString("UserEmail", auth.User.Email);
        _httpContextAccessor.HttpContext?.Session.SetString("UserName", auth.User.Nome);
        _httpContextAccessor.HttpContext?.Session.SetString("UserId", auth.User.Id);
        _httpContextAccessor.HttpContext?.Session.SetString("UserFoto", auth.User.Foto ?? "");
        _httpContextAccessor.HttpContext?.Session.SetString("UserPerfil", auth.User.Perfil);
    }

    private string TryParseError(string content)
    {
        try
        {
            var err = JsonSerializer.Deserialize<ErrorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return err?.Message;
        }
        catch { return null; }
    }

    public void Logout() => _httpContextAccessor.HttpContext?.Session.Clear();
    public bool IsAuthenticated() => !string.IsNullOrEmpty(GetUserToken());
    public string GetUserToken() => _httpContextAccessor.HttpContext?.Session.GetString("JWTToken") ?? string.Empty;
    public string GetUserName() => _httpContextAccessor.HttpContext?.Session.GetString("UserName") ?? string.Empty;
    public string GetUserEmail() => _httpContextAccessor.HttpContext?.Session.GetString("UserEmail") ?? string.Empty;
    public string GetUserFoto() => _httpContextAccessor.HttpContext?.Session.GetString("UserFoto") ?? string.Empty;
    public string GetUserPerfil() => _httpContextAccessor.HttpContext?.Session.GetString("UserPerfil") ?? string.Empty;
}
