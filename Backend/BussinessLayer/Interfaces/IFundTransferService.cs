using System;
using ModelLayer.Models;
using System.Threading.Tasks;
using ModelLayer.DTOs;

namespace BussinessLayer.Interfaces;

public interface IFundTransferService
    {
        Task<bool> AddPayeeAsync(PayeeDTO payee);
        Task<bool> TransferFundsAsync(FundTransferRequest request);
    }
