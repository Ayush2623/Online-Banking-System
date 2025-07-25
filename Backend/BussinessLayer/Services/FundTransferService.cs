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
            // var payeeModel = new Payee
            // {
            //     UserId = payee.UserId,
            //     PayeeName = payee.PayeeName,
            //     PayeeAccountNumber = payee.PayeeAccountNumber,
            //     Nickname = payee.Nickname                
            // };
            return await _fundTransferRepository.AddPayeeAsync(payee);
        }

        public async Task<bool> TransferFundsAsync(FundTransferRequest request)
        {
            return await _fundTransferRepository.TransferFundsAsync(request);
        }
    }
