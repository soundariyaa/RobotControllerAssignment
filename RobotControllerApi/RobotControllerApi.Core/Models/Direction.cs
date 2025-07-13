using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RobotControllerApi.Core.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Direction
    {
        N,
        E,
        S,
        W
    }
}
