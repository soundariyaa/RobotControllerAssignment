using RobotControllerApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RobotControllerApi.Core.Models
{
    public class RobotRequest
    {
        public string Name { get; set; } = "Robo1";
        public int RoomId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Direction Facing { get; set; }
        public Room? Room { get; set; }
        public string Commands { get; set; } = string.Empty;
    }
}
