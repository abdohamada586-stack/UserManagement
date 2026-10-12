using UserManagement.Service.DTOs;

namespace UserManagement.Service.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterDto dto);

        Task<string?> LoginAsync(LoginDto dto);

        Task<UserDetailsDto?> GetUserAsync(string id);

        Task<bool> UpdateUserAsync(string id, UpdateUserDto dto);

        Task<bool> SetUserStatusAsync(string id, bool isActive);
    }
}