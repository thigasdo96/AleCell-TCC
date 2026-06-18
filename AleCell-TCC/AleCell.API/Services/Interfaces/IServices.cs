using AleCell.API.DTOs;

namespace AleCell.API.Services.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string subDirectory = "uploads");
    Task<bool> DeleteFileAsync(string filePath);
    string GetFileUrl(string filePath);
}

public interface IJwtService
{
    string GenerateToken(UserDto user);
}

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<UserDto> GetUserByIdAsync(string userId);
}
