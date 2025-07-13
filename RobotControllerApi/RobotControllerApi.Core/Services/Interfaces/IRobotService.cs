using RobotControllerApi.Core.Entities;
using RobotControllerApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControllerApi.Core.Services.Interfaces
{
   public interface IRobotService
    {
        ResponseReport RobotExecuteCommands(RobotRequest robot, int roomWidth, int roomHeight);
        Task<ResponseReport> ExecuteAndSaveRobotAsync(RobotRequest request);
    }
}
