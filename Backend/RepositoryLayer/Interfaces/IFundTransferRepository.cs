using System;
using ModelLayer.Models;
using System.Threading.Tasks;
using ModelLayer.DTOs;

namespace RepositoryLayer.Interfaces;

public interface IFundTransferRepository
    {
        Task<bool> AddPayeeAsync(PayeeDTO payee);
        Task<bool> TransferFundsAsync(FundTransferRequest request);
    }
