using RobotControllerApi.Core.Entities;
using RobotControllerApi.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControllerApi.Infrastructure.Repositories
{
   public class RobotRepository : IRobotRepository
    {
        private readonly RoboDbContext _context;

        public RobotRepository(RoboDbContext context)
        {
            _context = context;
        }

        public async Task SaveRobotAsync(Robot robot)
        {
            _context.Robots.Add(robot);
            await _context.SaveChangesAsync();
        }
    }
}
