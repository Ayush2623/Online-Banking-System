using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BussinessLayer.Interfaces;
using ModelLayer.Models;
using ModelLayer.DTOs;

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
            return BadRequest(ModelState);
        }

        var result = await _fundTransferService.AddPayeeAsync(payee);
        if (!result)
        {
            return BadRequest("Failed to add payee.");
        }

        return Ok("Payee added successfully.");
    }
    
    // Fund Transfer
    [HttpPost("transfer-funds")]
    public async Task<IActionResult> TransferFunds([FromBody] FundTransferRequest request)
    {
        var result = await _fundTransferService.TransferFundsAsync(request);
        if (!result)
        {
            return BadRequest("Fund transfer failed. Please check the details.");
        }

        return Ok("Funds transferred successfully.");
    }
}
}
