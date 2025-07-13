using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotControllerApi.Core.Exceptions;
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
        private readonly IValidator<RobotRequest> _validator;

        public RobotExecutionController(ILogger<RobotExecutionController> logger, IRobotService robotService, RoboDbContext roboDbContext, IValidator<RobotRequest> validator)
        {
            _logger = logger;
            _robotService = robotService;
            _context = roboDbContext;
            _validator = validator;
        }

        [HttpPost("SaveRobot")]
        public async Task<ActionResult<ResponseReport>> SaveRobot([FromBody] RobotRequest robotRequest)
        {            
            try
            {
                var validationResult = await _validator.ValidateAsync(robotRequest);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                    return BadRequest(ModelState);
                }

                var report = await _robotService.ExecuteAndSaveRobotAsync(robotRequest);

                if (report == null)
                {
                    _logger.LogError("Failed to save robot execution.");
                    return StatusCode(500, "Failed to save robot execution.");
                }

                _logger.LogInformation("Robot executed and saved: {@robot}", report);
                return Ok(report);

            }
            catch (DbException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during robot execution");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
