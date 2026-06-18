using System.Text;
using System.Text.Json;
using AleCell.UI.Models;
using Microsoft.Extensions.Options;

namespace AleCell.UI.Services.Implementations;

public abstract class BaseApiService
{
    protected readonly HttpClient _httpClient;
    protected readonly ApiSettings _apiSettings;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    protected BaseApiService(
        HttpClient httpClient,
        IOptions<ApiSettings> apiSettings,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings.Value;
        _httpContextAccessor = httpContextAccessor;
        _httpClient.BaseAddress = new Uri(_apiSettings.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds);
    }

    protected async Task<T> GetAsync<T>(string endpoint)
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync(endpoint);
        return await HandleResponse<T>(response);
    }

    protected async Task<bool> DeleteAsync(string endpoint)
    {
        AddAuthHeader();
        var response = await _httpClient.DeleteAsync(endpoint);
        return response.IsSuccessStatusCode;
    }

    protected void AddAuthHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    private async Task<T> HandleResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        try
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                _httpContextAccessor.HttpContext?.Session.Clear();
            throw new HttpRequestException(errorResponse?.Message ?? $"Erro: {response.StatusCode}");
        }
        catch (JsonException)
        {
            throw new HttpRequestException($"Erro {response.StatusCode}: {content}");
        }
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}
