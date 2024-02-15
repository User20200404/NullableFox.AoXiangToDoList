using NullableFox.AoXiangToDoList.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Transmission
{
    /// <summary>
    /// 表示来自后端的通知包。
    /// </summary>
    internal class NotificationPacket
    {
        public string Content { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
