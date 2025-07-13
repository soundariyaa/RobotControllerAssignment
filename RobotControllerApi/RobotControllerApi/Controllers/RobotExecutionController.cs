using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotControllerApi.Core.Models;
using RobotControllerApi.Core.Services.Interfaces;
using RobotControllerApi.Core.Validators;
using RobotControllerApi.Infrastructure.Repositories;
using RobotControllerApi.Mappers;

namespace RobotControllerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RobotExecutionController : ControllerBase
    {
        private readonly ILogger<RobotExecutionController> _logger;
        private readonly IRobotService _robotService;
        private readonly RoboDbContext _context;       
        public RobotExecutionController(ILogger<RobotExecutionController> logger, IRobotService robotService, RoboDbContext roboDbContext)
        {
            _logger = logger;
            _robotService = robotService;
            _context = roboDbContext;
            
        }

        [HttpPost("SaveRobot")]
        public async Task<ActionResult<ResponseReport>> SaveRobot([FromBody] RobotRequest robotRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var validationResult = await new RobotRequestValidator().ValidateAsync(robotRequest);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                    return BadRequest(ModelState);
                }

                var savedRobot = await _robotService.ExecuteAndSaveRobotAsync(robotRequest);

                if (savedRobot == null)
                {
                    _logger.LogError("Failed to save robot execution.");
                    return StatusCode(500, "Failed to save robot execution.");
                }

                _logger.LogInformation("Robot executed and saved: {@robot}", savedRobot);
                return Ok(savedRobot);
                                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during robot execution");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
