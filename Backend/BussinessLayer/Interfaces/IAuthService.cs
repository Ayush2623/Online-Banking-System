using ModelLayer.DTOs;
using ModelLayer.Models;

namespace BussinessLayer.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(AuthDTO authDto);
        Task<string> LoginAsync(LoginDTO loginDto);
        Task<Auth> GetUserAsync(string username); // Add this method
    }
}