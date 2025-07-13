using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RobotControllerApi.Controllers;
using RobotControllerApi.Core.Entities;
using RobotControllerApi.Core.Models;
using RobotControllerApi.Core.Services.Interfaces;
using RobotControllerApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControllerApi.Test.Controllers
{
   public class RobotExecutionControllerTests
    {
        private readonly Mock<ILogger<RobotExecutionController>> _mockLogger;
        private readonly Mock<IRobotService> _mockRobotService;
        private readonly Mock<RoboDbContext> _mockRoboDbContext;
        private readonly RobotExecutionController _controller;

        public RobotExecutionControllerTests()
        {
            _mockLogger = new Mock<ILogger<RobotExecutionController>>();
            _mockRobotService = new Mock<IRobotService>();
            _mockRoboDbContext = new Mock<RoboDbContext>();
            _controller = new RobotExecutionController(_mockLogger.Object, _mockRobotService.Object, _mockRoboDbContext.Object);
        }

        [Fact]
        public async Task SaveRobot_ValidRequest_ReturnsOk()
        {
            var request = new RobotRequest
            {
                Name = "TestBot",
                RoomId = 1,
                X = 0,
                Y = 0,
                Facing = Direction.N,
                Commands = "MRL",
                Room = new Room { Width = 5, Height = 5 }
            };

            var expectedReport = new ResponseReport
            {
                X = 1,
                Y = 0,
                Facing = Direction.E
            };

            _mockRobotService.Setup(s => s.ExecuteAndSaveRobotAsync(request))
                             .ReturnsAsync(expectedReport);

            var result = await _controller.SaveRobot(request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualReport = Assert.IsType<ResponseReport>(okResult.Value);

            Assert.Equal(expectedReport.X, actualReport.X);
        }
    }
}
