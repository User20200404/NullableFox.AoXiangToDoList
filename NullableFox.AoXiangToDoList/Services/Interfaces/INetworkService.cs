using NullableFox.AoXiangToDoList.Services.Transmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Interfaces
{
    internal interface INetworkService
    {
        public Task<ResponsePacket> RequestAsync(RequestPacket packet);
        public Task<ResponsePacket> RequestAsync(RequestType type, string content)
        {
            return RequestAsync(new RequestPacket { Content = content, RequestType = type });
        }

        public event EventHandler<NotificationPacket> NotificationReceived;
        public event EventHandler NotificationServiceReconnected;
        public event EventHandler NetworkReconnected;
    }
}
