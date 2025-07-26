using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BussinessLayer.Interfaces;
using ModelLayer.Models;
using ModelLayer.DTOs;
using System.Collections.Generic;

namespace OnlineBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundTransferController : ControllerBase
{
    private readonly IFundTransferService _fundTransferService;

    public FundTransferController(IFundTransferService fundTransferService)
    {
        _fundTransferService = fundTransferService;
    }

    // Add Payee
    [HttpPost("add-payee")]
    public async Task<IActionResult> AddPayee([FromBody] PayeeDTO payee)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse.Error("Invalid data provided."));
        }

        var result = await _fundTransferService.AddPayeeAsync(payee);
        if (!result)
        {
            return BadRequest(ApiResponse.Error("Failed to add payee. Please check if the account number exists."));
        }

        return Ok(ApiResponse.Success("Payee added successfully."));
    }
    
    // Get Payees by Account Number
    [HttpGet("payees/{accountNumber}")]
    public async Task<IActionResult> GetPayees(long accountNumber)
    {
        var payees = await _fundTransferService.GetPayeesByAccountNumberAsync(accountNumber);
        return Ok(ApiResponse<List<Payee>>.SuccessResponse("Payees retrieved successfully.", payees));
    }
    
    // Fund Transfer
    [HttpPost("transfer-funds")]
    public async Task<IActionResult> TransferFunds([FromBody] FundTransferRequest request)
    {
        var result = await _fundTransferService.TransferFundsAsync(request);
        if (!result)
        {
            return BadRequest(ApiResponse.Error("Fund transfer failed. Please check the details."));
        }

        return Ok(ApiResponse.Success("Funds transferred successfully."));
    }
}
}
