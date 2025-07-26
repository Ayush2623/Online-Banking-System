using BussinessLayer.Interfaces;
using ModelLayer.DTOs;
using ModelLayer.Models;
using RepositoryLayer.Interfaces;
using System.Threading.Tasks;

namespace BussinessLayer.Services
{
    public class NetBankingService : INetBankingService
    {
        private readonly INetBankingRepository _netBankingRepository;

        public NetBankingService(INetBankingRepository netBankingRepository)
        {
            _netBankingRepository = netBankingRepository;
        }

        public async Task<string> RegisterForNetBanking(long accountNumber, string username, string password)
        {
            // Check if the account has NetBanking enabled
            var account = await _netBankingRepository.GetAccountByIdAsync(accountNumber);
            if (account == null)
            {
                return "Account not found.";
            }

            if (!account.IsNetBankingEnabled)
            {
                // Automatically enable NetBanking for the account
                account.IsNetBankingEnabled = true;
                await _netBankingRepository.UpdateAccountAsync(account);
            }

            // Create a NetBanking record
            var netBanking = new NetBanking
            {
                AccountNumber = accountNumber,
                Username = username,
                Password = password,
                CreatedAt = DateTime.UtcNow
            };

            await _netBankingRepository.AddNetBankingAsync(netBanking);
            return "NetBanking registration successful.";
        }

        public async Task<bool> EnableNetBankingAsync(EnableNetBankingRequest request)
        {
            var netBanking = new NetBanking
            {
                AccountNumber= request.AccountNumber,
                // AccountId = request.AccountId, // Assuming you have a way to get the AccountId from the account number
                Username = request.Username,
                Password = request.Password,
                CreatedAt = DateTime.UtcNow
            };
            

            return await _netBankingRepository.AddNetBankingAsync(netBanking);
        }

        public async Task<bool> UpdatePasswordAsync(UpdateNetBankingPasswordRequest request)
        {
            return await _netBankingRepository.UpdatePasswordAsync(request.AccountId, request.OldPassword, request.NewPassword);
        }

        public async Task<NetBankingDetailsDTO> GetNetBankingDetailsAsync(long accountNumber)
        {
            var netBanking = await _netBankingRepository.GetNetBankingByAccountIdAsync(accountNumber);
            if (netBanking == null) return null;

            return new NetBankingDetailsDTO
            {
                AccountNumber = netBanking.AccountNumber,
                Username = netBanking.Username,
                CreatedAt = netBanking.CreatedAt
            };
        }
    }
}