using BussinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using ModelLayer.Models;
using Moq;

namespace TestProject1;

public class Tests
{
    private Mock<IAccountService> _mockAccountService;
    private Mock<IAuthService> _mockAuthService;
    private Mock<ISmtpService> _mockSmtpService;
    private AccountController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockAccountService = new Mock<IAccountService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockSmtpService = new Mock<ISmtpService>();
            _controller = new AccountController(_mockAccountService.Object, _mockSmtpService.Object, _mockAuthService.Object);
        }

        // Test for OpenAccount
        [Test]
        public async Task OpenAccount_ValidRequest_ReturnsOkResult()
        {
            var pendingAccountDto = new PendingAccountDTO
            {
                Name = "John Doe",
                Email = "john.doe@gmail.com",
                MobileNumber = "9876543210",
                EnableNetBanking = true,
                UserId = 0,
            };

            _mockAccountService
                .Setup(service => service.OpenAccountAsync(It.IsAny<PendingAccountDTO>()))
                .ReturnsAsync("Success");

            var result = await _controller.OpenAccount(pendingAccountDto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Success", okResult.Value);
        }

        // Test for ViewAccountByUserId
        [Test]
        public async Task ViewAccountByUserId_ValidUserId_ReturnsOkResult()
        {
            var userId = 1;
            var accountDto = new AccountDTO
            {
                AccountId = 1,
                Name = "John Doe",
                Email = "john.doe@gmail.com",
                MobileNumber = "9876543210",
                EnableNetBanking = true
            };

            _mockAccountService
                .Setup(service => service.ViewAccountByUserIdAsync(userId))
                .ReturnsAsync(accountDto);

            var result = await _controller.ViewAccountByUserId(userId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(accountDto, okResult.Value);
        }

        [Test]
        public async Task ViewAccountByUserId_InvalidUserId_ReturnsNotFound()
        {
            var userId = 999;

            _mockAccountService
                .Setup(service => service.ViewAccountByUserIdAsync(userId))
                .ReturnsAsync((AccountDTO)null);

            var result = await _controller.ViewAccountByUserId(userId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Account not found.", notFoundResult.Value);
        }

        // Test for UpdateAccount
        [Test]
        public async Task UpdateAccount_ValidRequest_ReturnsOkResult()
        {
            var accountNumber = 1234567890;
            var updatedAccount = new UpdateAccountDTO
            {
                Name = "Updated Name",
                Email = "updated.email@gmail.com"
            };

            _mockAccountService
                .Setup(service => service.UpdateAccountAsync(accountNumber, updatedAccount))
                .ReturnsAsync(true);

            var result = await _controller.UpdateAccount(accountNumber, updatedAccount);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Account updated successfully.", okResult.Value);
        }

        [Test]
        public async Task UpdateAccount_InvalidRequest_ReturnsNotFound()
        {
            var accountNumber = 1234567890;
            var updatedAccount = new UpdateAccountDTO
            {
                Name = "Updated Name",
                Email = "updated.email@gmail.com"
            };

            _mockAccountService
                .Setup(service => service.UpdateAccountAsync(accountNumber, updatedAccount))
                .ReturnsAsync(false);

            var result = await _controller.UpdateAccount(accountNumber, updatedAccount);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Account not found or update failed.", notFoundResult.Value);
        }

        // Test for ForgotPassword
        [Test]
        public async Task ForgotPassword_ValidRequest_ReturnsOkResult()
        {
            var request = new ModelLayer.DTOs.ForgotPasswordRequest
            {
                UserId = 1,
                MobileNumber = "9876543210",
                Email = "john.doe@gmail.com"
            };

            _mockAccountService
                .Setup(service => service.VerifyMobileNumberAsync(request.UserId, request.MobileNumber))
                .ReturnsAsync(true);

            _mockAccountService
                .Setup(service => service.SaveResetTokenAsync(request.UserId, It.IsAny<string>()))
                .ReturnsAsync(true);

            var result = await _controller.ForgotPassword(request);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Password reset link has been sent to your registered email.", okResult.Value);
        }

        // Test for ForgotUserId
        [Test]
        public async Task ForgotUserId_ValidRequest_ReturnsOkResult()
        {
            var request = new ModelLayer.DTOs.ForgotUserIdRequest
            {
                UserId = 1,
                MobileNumber = "9876543210",
                Email = "john.doe@gmail.com"
            };

            _mockAccountService
                .Setup(service => service.VerifyMobileNumberAsync(request.UserId, request.MobileNumber))
                .ReturnsAsync(true);

            _mockAccountService
                .Setup(service => service.GetUserIdByEmailAsync(request.Email))
                .ReturnsAsync("User123");

            var result = await _controller.ForgotUserId(request);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Your User ID has been sent to your registered email.", okResult.Value);
        }

        // Test for SetNewPassword
        [Test]
        public async Task SetNewPassword_ValidRequest_ReturnsOkResult()
        {
            var request = new SetNewPasswordRequest
            {
                UserId = 1,
                NewPassword = "NewPassword123",
                ResetToken = "ValidToken"
            };

            _mockAccountService
                .Setup(service => service.SetNewPasswordAsync(request.UserId, request.NewPassword, request.ResetToken))
                .ReturnsAsync(true);

            var result = await _controller.SetNewPassword(request);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Password updated successfully.", okResult.Value);
        }
    }
