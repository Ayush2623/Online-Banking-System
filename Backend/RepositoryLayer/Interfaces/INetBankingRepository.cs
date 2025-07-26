using Microsoft.EntityFrameworkCore;
using ModelLayer.Models;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface INetBankingRepository
    {
        Task<Account> GetAccountByIdAsync(long accountNumber);
        Task<bool> AddNetBankingAsync(NetBanking netBanking);
        

       Task<bool> UpdatePasswordAsync(long accountNumber, string oldPassword, string newPassword);
        

        Task<NetBanking> GetNetBankingByAccountIdAsync(long accountNumber);
        Task<bool> UpdateAccountAsync(Account account);
       
    }
}