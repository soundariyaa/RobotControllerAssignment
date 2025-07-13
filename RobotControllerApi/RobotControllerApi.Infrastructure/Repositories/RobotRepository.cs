using Microsoft.Extensions.Logging;
using RobotControllerApi.Core.Entities;
using RobotControllerApi.Core.Exceptions;
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
        private readonly ILogger<RobotRepository> _logger;

        public RobotRepository(RoboDbContext context, ILogger<RobotRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SaveRobotAsync(Robot robot)
        {
            try
            {
                _context.Robots.Add(robot);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving robot to the database.");
                throw new DbException("Something went wrong saving the Robot");
            }
        }
    }
}
