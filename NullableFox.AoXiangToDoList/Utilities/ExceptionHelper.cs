using NullableFox.AoXiangToDoList.Exceptions;
using NullableFox.AoXiangToDoList.Services.Transmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Utilities
{
    internal class ExceptionHelper
    {
        public static void ThrowOnTransFailure(ResponsePacket response, string title = "发生未指定错误")
        {
            if (response.Status != ResponseStatus.Failure) return;

            throw new ApplicationShowableException() { Description = $"来自服务器的消息：{response.Message}", Title = title };
        }
    }
}
