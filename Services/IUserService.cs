using DotNetCore_New.Model;

namespace DotNetCore_New.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(UserDTO userDTO);
        Task<List<UserReadOnlyDTO>> GetUsersAsync();
        Task<UserReadOnlyDTO> GetUserByIdAsync(int id);
        Task<UserReadOnlyDTO> GetUserByNameAsync(string name);
        Task<bool> UpdateUserAsync(UserDTO userDTO);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UpdateUserPasswordAsync(int id, string password);
        public (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string password);
    }
}
