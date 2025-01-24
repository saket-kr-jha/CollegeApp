using DotNetCore_New.Model;

namespace DotNetCore_New.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(UserDTO userDTO);
        Task<List<UserReadOnlyDTO>> GetUsersAsync();
        Task<UserReadOnlyDTO> GetUserByIdAsync(int id);
        Task<UserReadOnlyDTO> GetUserByNameAsync(string name);
        public (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string password);
    }
}
