using System;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Models;
using RepositoryLayer.Interfaces;
using System.Threading.Tasks;
using ModelLayer.DTOs;

namespace RepositoryLayer.Repositories;

public class FundTransferRepository : IFundTransferRepository
    {
        private readonly OnlineBankDbContext _context;

        public FundTransferRepository(OnlineBankDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPayeeAsync(Payee payee)
        {
            // Check if the payee account exists
            var payeeAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == payee.PayeeAccountNumber);
            if (payeeAccount == null)
            {
                return false; // Payee account does not exist
            }
            
            // Check if user's account exists
            var userAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == payee.AccountNumber);
            if (userAccount == null)
            {
                return false; // User's account does not exist
            }

            await _context.Payees.AddAsync(payee);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> TransferFundsAsync(FundTransferRequest request)
        {
            var fromAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.FromAccountNumber);
            var toAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.ToAccountNumber);

            if (fromAccount == null || toAccount == null || fromAccount.Balance < request.Amount)
            {
                return false;
            }

            // Deduct from sender's account
            fromAccount.Balance -= request.Amount;

            // Add to receiver's account
            toAccount.Balance += request.Amount;

            // Add transaction record
            var transaction = new Transaction
            {
                FromAccountNumber = request.FromAccountNumber,
                ToAccountNumber = request.ToAccountNumber,
                Amount = request.Amount,
                TransactionType = request.TransferMode,
                TransactionDate = System.DateTime.Now,
                Remarks = request.Remarks
            };
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Payee>> GetPayeesByAccountNumberAsync(long accountNumber)
        {
            return await _context.Payees
                .Where(p => p.AccountNumber == accountNumber)
                .OrderBy(p => p.PayeeName)
                .ToListAsync();
        }
    }

