using RobotControllerApi.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotControllerApi.Core.Entities;
using RobotControllerApi.Core.Services;
using RobotControllerApi.Core.Models;
using RobotControllerApi.Infrastructure.Repositories;
using RobotControllerApi.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace RobotControllerApi.Infrastructure.Services.Implementations
{
    public class RobotService : IRobotService
    {
        private readonly ILogger<RobotService> _logger;

        private readonly IRobotRepository _robotRepository;
      
        public RobotService(ILogger<RobotService> logger,IRobotRepository robotRepository)
        {
            _logger = logger;
            _robotRepository = robotRepository ?? throw new ArgumentNullException(nameof(robotRepository));
           
        }
        public ResponseReport RobotExecuteCommands(RobotRequest robot, int roomWidth, int roomHeight)
        {
            
            int x = robot.X;
            int y = robot.Y;
            Direction facing = robot.Facing;
            foreach (char command in robot.Commands)
            {
                switch (command)
                {
                    case 'L':
                        facing = TurnLeft(facing);
                        break;
                    case 'R':
                        facing = TurnRight(facing);
                        break;
                    case 'F':
                        (x, y) = MoveForward(x, y, facing, roomWidth, roomHeight);
                        break;
                    default:
                        throw new ArgumentException($"Invalid command: {command}");
                }
            }

            robot.X = x;
            robot.Y = y;
            robot.Facing = facing;
            return new ResponseReport
            {
                X = x,
                Y = y,
                Facing = facing
            };
        }

        public async Task<ResponseReport> ExecuteAndSaveRobotAsync(RobotRequest request)
        {          
            var result = RobotExecuteCommands(request, request.Room.Width, request.Room.Height);

            var robot = new Robot
            {
                Name = request.Name,
                RoomId = request.RoomId,
                X = result.X,
                Y = result.Y,
                Facing = result.Facing,
                ExecutedAt = DateTime.UtcNow
            };        
                        
            await _robotRepository.SaveRobotAsync(robot);

            return result;
        }

        private Direction TurnLeft(Direction facing) => facing switch
        {
            Direction.N => Direction.W,
            Direction.W => Direction.S,
            Direction.S => Direction.E,
            Direction.E => Direction.N,
            _ => throw new InvalidOperationException("Invalid direction")
        };

        private Direction TurnRight(Direction facing) => facing switch
        {
            Direction.N => Direction.E,
            Direction.E => Direction.S,
            Direction.S => Direction.W,
            Direction.W => Direction.N,
            _ => throw new InvalidOperationException("Invalid direction")
        };

        private (int x, int y) MoveForward(int x, int y, Direction facing, int width, int height)
        { 
            int newX = x;
            int newY = y;
            switch (facing)
            {
                case Direction.N: newY--; break;
                case Direction.E: newX++; break;
                case Direction.S: newY++; break;
                case Direction.W: newX--; break;
            }

            if (newX < 0 || newX >= width || newY < 0 || newY >= height)
                throw new InvalidOperationException("Robot moved outside the room bounds.");

            _logger.LogInformation($"Robot moved to position ({newX}, {newY}) facing {facing}.");
            return (newX, newY);                     
        }        
    }
}
