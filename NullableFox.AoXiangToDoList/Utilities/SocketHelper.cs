using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList
{
    public static class SocketHelper
    {
        public static void SendInt32(this Socket socket, int value)
        {
            byte[] buffer = new byte[4];
            buffer[3] = (byte)value;
            buffer[2] = (byte)(value >> 8);
            buffer[1] = (byte)(value >> 16);
            buffer[0] = (byte)(value >> 24);
            socket.Send(buffer);
        }

        public static int ReceiveInt32(this Socket socket)
        {
            byte[] buffer = new byte[sizeof(int)];
            int recBytes = 0;
            while (recBytes < 4)
                recBytes += socket.Receive(buffer, recBytes, sizeof(int) - recBytes, SocketFlags.None);
            return ((buffer[0] << 24) + (buffer[1] << 16) + (buffer[2] << 8) + (buffer[3] << 0));
        }

        public static void SendString(this Socket socket, string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            socket.Send(buffer);
        }

        public static string ReceiveString(this Socket socket, int byteLength)
        {
            byte[] buffer = new byte[byteLength];
            int recBytes = 0;
            while (recBytes < byteLength)
            {
                recBytes += socket.Receive(buffer, recBytes, byteLength - recBytes, SocketFlags.None);
            }
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
