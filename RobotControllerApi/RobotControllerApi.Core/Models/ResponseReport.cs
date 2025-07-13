using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControllerApi.Core.Models
{
    public class ResponseReport
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Facing { get; set; }
    }
}
