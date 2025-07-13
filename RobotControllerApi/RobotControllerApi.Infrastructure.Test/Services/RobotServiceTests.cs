using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RobotControllerApi.Core.Models;
using RobotControllerApi.Core.Entities;
using RobotControllerApi.Infrastructure.Services.Implementations;
using RobotControllerApi.Core.Repositories;
using System;

public class RobotServiceTests
{
    private readonly Mock<IRobotRepository> _robotRepositoryMock;
    private readonly Mock<ILogger<RobotService>> _loggerMock;
    private readonly RobotService _service;

    public RobotServiceTests()
    {
        _robotRepositoryMock = new Mock<IRobotRepository>();
        _loggerMock = new Mock<ILogger<RobotService>>();
        _service = new RobotService(_loggerMock.Object, _robotRepositoryMock.Object);
    }

    [Fact]
    public void RobotExecuteCommands_Should_Process_Valid_Commands()
    {
        var request = new RobotRequest
        {
            X = 0,
            Y = 0,
            Facing = Direction.N,
            Commands = "RFF",
            Room = new Room { Width = 5, Height = 5 }
        };

        var result = _service.RobotExecuteCommands(request, 5, 5);

        Assert.Equal(2, result.X);
        Assert.Equal(0, result.Y);
        Assert.Equal(Direction.E, result.Facing);
    }

    [Fact]
    public void RobotExecuteCommands_Should_Throw_On_Invalid_Command()
    {
        var request = new RobotRequest
        {
            X = 0,
            Y = 0,
            Facing = Direction.N,
            Commands = "X",
            Room = new Room { Width = 5, Height = 5 }
        };

        var ex = Assert.Throws<ArgumentException>(() =>
       _service.RobotExecuteCommands(request, 5, 5));

        Assert.Contains("Invalid command", ex.Message);
    }

    [Fact]
    public void RobotExecuteCommands_Should_Throw_When_Moving_Outside_Bounds()
    {
        var request = new RobotRequest
        {
            X = 0,
            Y = 0,
            Facing = Direction.W,
            Commands = "F",
            Room = new Room { Width = 5, Height = 5 }
        };

        Assert.Throws<InvalidOperationException>(() => _service.RobotExecuteCommands(request, 5, 5));
    }

    [Fact]
    public async Task ExecuteAndSaveRobotAsync_Should_Save_And_Return_Result()
    {
        var request = new RobotRequest
        {
            Name = "TestBot",
            RoomId = 1,
            X = 2,
            Y = 1,
            Facing = Direction.N,
            Commands = "LFF",
            Room = new Room { Width = 5, Height = 5 }
        };

        _robotRepositoryMock.Setup(repo => repo.SaveRobotAsync(It.IsAny<Robot>()))
                            .Returns(Task.CompletedTask);

        var result = await _service.ExecuteAndSaveRobotAsync(request);

        Assert.NotNull(result);
        Assert.Equal(0, result.X);
        Assert.Equal(1, result.Y);
        Assert.Equal(Direction.W, result.Facing);

        _robotRepositoryMock.Verify(r => r.SaveRobotAsync(It.IsAny<Robot>()), Times.Once);
    }
       
}
