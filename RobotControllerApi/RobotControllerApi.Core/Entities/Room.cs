using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControllerApi.Core.Entities
{
   public class Room
    {
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }      
        public bool IsInsideBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}
