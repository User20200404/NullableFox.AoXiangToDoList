using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Transmission
{
    internal class ResponsePacket
    {
        public string Message { get; set; }
        public string Content { get; set; }
        public ResponseStatus Status { get; set; }
        public string ToJsonString() => Utilities.JsonHelper.ToJsonString(this);
    }
}
