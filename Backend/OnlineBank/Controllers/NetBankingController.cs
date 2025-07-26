using Microsoft.AspNetCore.Mvc;
using BussinessLayer.Interfaces;
using ModelLayer.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace OnlineBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class NetBankingController : ControllerBase
    {
        private readonly INetBankingService _netBankingService;

        public NetBankingController(INetBankingService netBankingService)
        {
            _netBankingService = netBankingService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterForNetBanking([FromBody] NetBankingDTO request)
        {
            var result = await _netBankingService.RegisterForNetBanking(request.AccountNumber, request.Username, request.Password);
            if (result == "NetBanking registration successful.")
            {
                return Ok(ApiResponse.Success(result));
            }

            return BadRequest(ApiResponse.Error(result));
        }
        // // Enable NetBanking
        // [HttpPost("enable")]
        // public async Task<IActionResult> EnableNetBanking([FromBody] EnableNetBankingRequest request)
        // {
        //     var result = await _netBankingService.EnableNetBankingAsync(request);
        //     if (!result)
        //     {
        //         return BadRequest("Failed to enable NetBanking. Please check the details.");
        //     }

        //     return Ok("NetBanking enabled successfully.");
        // }

        // Update NetBanking Password
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateNetBankingPasswordRequest request)
        {
            var result = await _netBankingService.UpdatePasswordAsync(request);
            if (!result)
            {
                return BadRequest(ApiResponse.Error("Failed to update NetBanking password. Please check the details."));
            }

            return Ok(ApiResponse.Success("NetBanking password updated successfully."));
        }

        // Get NetBanking Details
        [HttpGet]
        public async Task<IActionResult> GetNetBankingDetails(long accountNumber)
        {
            var netBankingDetails = await _netBankingService.GetNetBankingDetailsAsync(accountNumber);
            if (netBankingDetails == null)
            {
                return NotFound(ApiResponse.Error("NetBanking details not found for the specified account."));
            }

            return Ok(ApiResponse<object>.SuccessResponse("NetBanking details retrieved successfully.", netBankingDetails));
        }
    }
}