using System;
using BussinessLayer.Interfaces;
using RepositoryLayer.Interfaces;
using ModelLayer.Models;
using System.Threading.Tasks;
using ModelLayer.DTOs;

namespace BussinessLayer.Services;

public class FundTransferService : IFundTransferService
    {
        private readonly IFundTransferRepository _fundTransferRepository;

        public FundTransferService(IFundTransferRepository fundTransferRepository)
        {
            _fundTransferRepository = fundTransferRepository;
        }

        public async Task<bool> AddPayeeAsync(PayeeDTO payee)
        {
            var payeeModel = new Payee
            {
                PayeeName = payee.PayeeName,
                PayeeAccountNumber = payee.PayeeAccountNumber,
                AccountNumber = payee.AccountNumber, // User's account who's adding this payee
                Nickname = payee.Nickname,
                CreatedAt = DateTime.Now
            };
            return await _fundTransferRepository.AddPayeeAsync(payeeModel);
        }

        public async Task<bool> TransferFundsAsync(FundTransferRequest request)
        {
            return await _fundTransferRepository.TransferFundsAsync(request);
        }

        public async Task<List<Payee>> GetPayeesByAccountNumberAsync(long accountNumber)
        {
            return await _fundTransferRepository.GetPayeesByAccountNumberAsync(accountNumber);
        }
    }
