using Microsoft.EntityFrameworkCore;
using ModelLayer.Models;
using RepositoryLayer.Interfaces;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories
{
    public class NetBankingRepository : INetBankingRepository
    {
        private readonly OnlineBankDbContext _context;

        public NetBankingRepository(OnlineBankDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAccountByIdAsync(long accountNumber)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }
        public async Task<bool> AddNetBankingAsync(NetBanking netBanking)
        {
            _context.NetBankings.Add(netBanking);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePasswordAsync(long accountNumber, string oldPassword, string newPassword)
        {
            var netBanking = await _context.NetBankings.FirstOrDefaultAsync(nb => nb.AccountNumber == accountNumber && nb.Password == oldPassword);
            if (netBanking == null) return false;

            netBanking.Password = newPassword;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<NetBanking> GetNetBankingByAccountIdAsync(long accountNumber)
        {
            return await _context.NetBankings.FirstOrDefaultAsync(nb => nb.AccountNumber == accountNumber);
        }
    }
}