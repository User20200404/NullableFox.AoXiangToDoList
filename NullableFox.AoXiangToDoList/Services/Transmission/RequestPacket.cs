using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Transmission
{
    internal class RequestPacket
    {
        public string Content { get; set; }
        public RequestType RequestType { get; set; }

        public string ToJsonString() => Utilities.JsonHelper.ToJsonString(this);
    }
}
