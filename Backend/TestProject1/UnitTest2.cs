using System;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BussinessLayer.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModelLayer.Models;


namespace TestProject1;

public class UnitTest2
{

        private Mock<IAccountService> _mockAccountService;
        private AdminController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockAccountService = new Mock<IAccountService>();
            _controller = new AdminController(_mockAccountService.Object);
        }

        // Test for GetPendingRequests
        [Test]
        public async Task GetPendingRequests_ReturnsOkResultWithPendingRequests()
        {
            // Arrange
            var pendingRequests = new List<PendingAccount>
            {
                new PendingAccount { Name = "Request1" },
                new PendingAccount { Name = "Request2" }
            }; // Mocked data
            _mockAccountService
                .Setup(service => service.GetPendingRequestsAsync())
                .ReturnsAsync(pendingRequests);

            // Act
            var result = await _controller.GetPendingRequests();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(pendingRequests, okResult.Value);
        }

        // Test for ApproveAccount
        [Test]
        public async Task ApproveAccount_ValidRequestId_ReturnsOkResult()
        {
            // Arrange
            var requestId = 1;
            var approvalResult = "Account approved successfully.";
            _mockAccountService
                .Setup(service => service.ApproveAccountAsync(requestId))
                .ReturnsAsync(approvalResult);

            // Act
            var result = await _controller.ApproveAccount(requestId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(approvalResult, okResult.Value);
        }

        // Test for RejectAccount
        [Test]
        public async Task RejectAccount_ValidRequestId_ReturnsOkResult()
        {
            // Arrange
            var requestId = 1;
            var rejectionResult = "Account rejected successfully.";
            _mockAccountService
                .Setup(service => service.RejectAccountAsync(requestId))
                .ReturnsAsync(rejectionResult);

            // Act
            var result = await _controller.RejectAccount(requestId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(rejectionResult, okResult.Value);
        }
    }

