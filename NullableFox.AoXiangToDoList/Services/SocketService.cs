using NullableFox.AoXiangToDoList.Exceptions;
using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.Services.Transmission;
using NullableFox.AoXiangToDoList.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services
{
    class SocketService : INetworkService
    {
        bool notificationContextRegisterRequired = true;
        int connectTimeout = 100;
        IPEndPoint endPoint;
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket notificationSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private RequestPacket notificationContextRegisterRequestPacket;
        private RequestPacket NotificationContextRegisterResponsePacket =>
            notificationContextRegisterRequestPacket ??= new RequestPacket()
            {
                Content = new NotificationContext()
                {
                    ContextName = NotificationContextName,
                    NotificationQueueMaxLength = 50
                }.ToJsonString(),
                RequestType = RequestType.RegisterNotificationContext
            };

        private string NotificationContextName => AppDomain.CurrentDomain.FriendlyName;

        public event EventHandler<NotificationPacket> NotificationReceived;
        public event EventHandler NotificationServiceReconnected;
        public event EventHandler NetworkReconnected;

        public SocketService(int backEndPort)
        {
            endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), backEndPort);
            clientSocket.ReceiveTimeout = 300;
            clientSocket.SendTimeout = 300;
            _ = NotificationLoopAsync();
        }

        /// <summary>
        /// 通知获取循环。
        /// </summary>
        async Task NotificationLoopAsync()
        {
            //构建获取通知的请求字符串。
            RequestPacket packet = new RequestPacket();
            packet.Content = NotificationContextName;
            packet.RequestType = RequestType.GetNotification;
            string getNotificationString = packet.ToJsonString();
            int length = Encoding.UTF8.GetByteCount(getNotificationString);

            //在新线程上运行接收通知的任务
            await Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {   //确保连接正常
                        await EnsureNotificationSocketConnection();
                        notificationSocket.SendInt32(length);
                        notificationSocket.SendString(getNotificationString);
                        int responseLength = notificationSocket.ReceiveInt32();
                        string responseJson = notificationSocket.ReceiveString(responseLength);
                        ResponsePacket response = JsonHelper.ObjectFromJsonString<ResponsePacket>(responseJson);
                        NotificationPacket notification = JsonHelper.ObjectFromJsonString<NotificationPacket>(response.Content);
                        NotificationReceived?.Invoke(this, notification);
                    }
                    catch (Exception ex)
                    {
                        //发生异常时关闭套接字并重新创建新的实例，在下次循环时自动尝试连接。
                        notificationSocket.Close();
                        notificationSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        Debug.WriteLine($"[{nameof(NotificationLoopAsync)}] 通知接收过程中发生错误：{ex.Message}");
                    }
                }
            });
        }

        async Task EnsureNotificationSocketConnection()
        {
            int count = 0;
            while (!notificationSocket.Connected)
            {
                try
                {
                    await notificationSocket.ConnectAsync(endPoint);
                    await RegisterNotificationContext();
                    NotificationServiceReconnected?.Invoke(this,EventArgs.Empty);
                }
                catch (SocketException)
                {
                    Debug.WriteLine($"[{nameof(EnsureNotificationSocketConnection)}] 尝试连接通知服务器，第{count++}次尝试");
                }
            }
        }

        async Task RegisterNotificationContext()
        {
            await RequestAsync(NotificationContextRegisterResponsePacket);
        }

        public Task<ResponsePacket> RequestAsync(RequestPacket packet)
        {
            return Task.Run(() =>
            {
                lock (clientSocket)
                {
                    if (!clientSocket.Connected)
                    {
                        var asyncResult = clientSocket.BeginConnect(endPoint, null, null);
                        asyncResult.AsyncWaitHandle.WaitOne(connectTimeout);
                        if (!clientSocket.Connected)
                        {
                            throw new ApplicationShowableException() { Description = $"连接在 {connectTimeout} ms内未能成功建立，后端程序可能未正常工作。", Title = "服务连接超时" };
                        }
                        Monitor.Exit(clientSocket); //即将调用外部事件处理器，它们可能会调用该本方法，为防止死锁，必须暂时退出套接字上的锁。
                        NetworkReconnected?.Invoke(this, EventArgs.Empty);
                        Monitor.Enter(clientSocket);
                    }

                    string requestJson = packet.ToJsonString();
                    int requestLength = Encoding.UTF8.GetBytes(requestJson).Length;
                    try
                    {
                        clientSocket.SendInt32(requestLength);
                        clientSocket.SendString(requestJson);

                        int responseLength = clientSocket.ReceiveInt32();
                        var responseJson = clientSocket.ReceiveString(responseLength);
                        return JsonHelper.ObjectFromJsonString<ResponsePacket>(responseJson);
                    }
                    catch
                    {
                        //连接中断，尝试重新连接并发送请求。
                        clientSocket.Close();
                        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        return RequestAsync(packet).GetAwaiter().GetResult();
                    }
                }
            });
        }
    }
}