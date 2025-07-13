using RobotControllerApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControllerApi.Core.Repositories
{
    public interface IRobotRepository
    {
        Task SaveRobotAsync(Robot robot);
    }
}
