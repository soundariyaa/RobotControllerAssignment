using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RobotControllerApi.Core.Entities;
using RobotControllerApi.Infrastructure.Repositories;

namespace RobotControllerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly RoboDbContext _context;
        public RoomController(ILogger<RoomController> logger, RoboDbContext roboDbContext)
        {
            _logger = logger;
            _context = roboDbContext;
        }
        [HttpPost("CreateRoom")]
        public async Task<IActionResult> CreateRoom([FromBody] Room roomRequest)
        {
            if (roomRequest == null)
            {
                return BadRequest("Invalid room request.");
            }
            if (roomRequest.Width <= 0 || roomRequest.Height <= 0)
            {
                return BadRequest("Width and Height must be greater than zero.");
            }
            _context.Rooms.Add(new Room
            {
                Width = roomRequest.Width,
                Height = roomRequest.Height
            });
            await _context.SaveChangesAsync();
            // Here you would typically save the room to a database or perform some action
            _logger.LogInformation($"Room saved with Width: {roomRequest.Width}, Height: {roomRequest.Height}");
            return Ok(new { Message = "Room saved successfully.", Width = roomRequest.Width, Height = roomRequest.Height });
        }

        [HttpGet("GetRoom/{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound($"Room with ID {id} not found.");
            }
            return Ok(room);
        }

        [HttpGet("GetAllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();
            if (rooms == null || !rooms.Any())
            {
                return NotFound("No rooms found.");
            }
            return Ok(rooms);
        }
    }
}
