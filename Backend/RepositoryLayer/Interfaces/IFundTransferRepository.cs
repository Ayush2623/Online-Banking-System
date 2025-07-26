using System;
using ModelLayer.Models;
using System.Threading.Tasks;
using ModelLayer.DTOs;

namespace RepositoryLayer.Interfaces;

public interface IFundTransferRepository
    {
        Task<bool> AddPayeeAsync(Payee payee);
        Task<bool> TransferFundsAsync(FundTransferRequest request);
        Task<List<Payee>> GetPayeesByAccountNumberAsync(long accountNumber);
    }
