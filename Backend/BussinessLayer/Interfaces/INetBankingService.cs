using BussinessLayer.Interfaces;
using ModelLayer.DTOs;
using ModelLayer.Models;

using System.Threading.Tasks;

namespace BussinessLayer.Interfaces
{
    public interface INetBankingService
    {
        Task<string> RegisterForNetBanking(long accountNumber, string username, string password);
        // Task<bool> EnableNetBankingAsync(EnableNetBankingRequest request);    
        Task<bool> UpdatePasswordAsync(UpdateNetBankingPasswordRequest request);
        Task<NetBankingDetailsDTO> GetNetBankingDetailsAsync(long accountNumber);
    }
}