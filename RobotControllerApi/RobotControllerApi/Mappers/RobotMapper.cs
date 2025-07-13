using RobotControllerApi.Core.Entities;
using RobotControllerApi.Core.Models;

namespace RobotControllerApi.Mappers
{
    public static class RobotMapper
    {
        public static Robot ToRobotEntity(this RobotRequest robotRequest)
        {
            if (robotRequest == null)
            {
                throw new ArgumentNullException(nameof(robotRequest), "RobotRequest cannot be null");
            }
            return new Robot
            {
                Name = robotRequest.Name,
                RoomId = robotRequest.RoomId,
                X = robotRequest.X,
                Y = robotRequest.Y,
                Facing = robotRequest.Facing,
                ExecutedAt = DateTime.UtcNow
            };
        }
    }
}
