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

        public async Task<bool> AddPayeeAsync(PayeeDTO payee)
        {
            var Account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == payee.PayeeAccountNumber);
            if (Account == null)
            {
                return false;
            }
            var payeeModel = new Payee
            {
                PayeeAccountNumber = payee.PayeeAccountNumber,
                PayeeName = payee.PayeeName,
                Nickname = payee.Nickname,
                CreatedAt = System.DateTime.Now,

            };

            await _context.Payees.AddAsync(payeeModel);
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
    }

