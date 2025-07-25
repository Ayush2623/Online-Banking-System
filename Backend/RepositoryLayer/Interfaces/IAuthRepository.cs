using ModelLayer.DTOs;
using ModelLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> UserExistsAsync(string username);
        Task RegisterUserAsync(Auth auth);
        Task<Auth> GetUserAsync(string username);
    }
}