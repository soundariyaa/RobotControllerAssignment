using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RobotControllerApi.Controllers;
using RobotControllerApi.Core.Entities;
using RobotControllerApi.Core.Models;
using RobotControllerApi.Core.Services.Interfaces;
using RobotControllerApi.Core.Validators;
using RobotControllerApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace RobotControllerApi.Test.Controllers
{
   public class RobotExecutionControllerTests
    {
        private readonly Mock<ILogger<RobotExecutionController>> _mockLogger;
        private readonly Mock<IRobotService> _mockRobotService;
        private readonly Mock<RoboDbContext> _mockRoboDbContext;
        private readonly RobotExecutionController _controller;
        private readonly Mock<IValidator<RobotRequest>> _mockValidator;

        public RobotExecutionControllerTests()
        {
            _mockLogger = new Mock<ILogger<RobotExecutionController>>();
            _mockRobotService = new Mock<IRobotService>();
            _mockRoboDbContext = new Mock<RoboDbContext>();
            _mockValidator = new Mock<IValidator<RobotRequest>>();
            
            _controller = new (_mockLogger.Object, _mockRobotService.Object, _mockRoboDbContext.Object,_mockValidator.Object);
        }

        [Fact]
        public async Task SaveRobot_ReturnsOk_WhenRequestIsValid()
        {
            // Arrange
            var request = new RobotRequest { Commands = "LFR", X = 0, Y = 0, Facing = Direction.N, Room = new Room { Width = 5, Height = 5 } };
            var validationResult = new ValidationResult();
            var responseReport = new ResponseReport { X = 1, Y = 1, Facing = Direction.E };

            _mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);
            _mockRobotService.Setup(s => s.ExecuteAndSaveRobotAsync(request)).ReturnsAsync(responseReport);

            // Act
            var result = await _controller.SaveRobot(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(responseReport, okResult.Value);
        }

        [Fact]
        public async Task SaveRobot_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var request = new RobotRequest();
            var errors = new List<ValidationFailure> { new ValidationFailure("Commands", "Required") };
            var validationResult = new ValidationResult(errors);

            _mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

            // Act
            var result = await _controller.SaveRobot(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task SaveRobot_ReturnsStatus500_WhenServiceReturnsNull()
        {
            // Arrange
            var request = new RobotRequest { Commands = "F", X = 0, Y = 0, Facing = Direction.N, Room = new Room { Width = 3, Height = 3 } };
            var validationResult = new ValidationResult();

            _mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);
            _mockRobotService.Setup(s => s.ExecuteAndSaveRobotAsync(request)).ReturnsAsync((ResponseReport)null);

            // Act
            var result = await _controller.SaveRobot(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task SaveRobot_ReturnsBadRequest_OnInvalidOperationException()
        {
            // Arrange
            var request = new RobotRequest { Commands = "F", X = 0, Y = 0, Facing = Direction.N, Room = new Room { Width = 3, Height = 3 } };
            var validationResult = new ValidationResult();

            _mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);
            _mockRobotService.Setup(s => s.ExecuteAndSaveRobotAsync(request)).ThrowsAsync(new InvalidOperationException("Invalid op"));

            // Act
            var result = await _controller.SaveRobot(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid op", badRequest.Value);
        }
    }   
}
