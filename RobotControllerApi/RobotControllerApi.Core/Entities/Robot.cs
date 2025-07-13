using RobotControllerApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControllerApi.Core.Entities
{
   public class Robot
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Robo1";
        public int RoomId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Facing { get; set; }
      
        public DateTime ExecutedAt { get; set; }
    }
}
