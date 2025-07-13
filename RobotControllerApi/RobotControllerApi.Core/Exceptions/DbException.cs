using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControllerApi.Core.Exceptions
{
    public class DbException(string exception) : Exception(exception)
    {
        private string ErrorCode { get; set; } = "DB_ERROR";
        
    }
}
