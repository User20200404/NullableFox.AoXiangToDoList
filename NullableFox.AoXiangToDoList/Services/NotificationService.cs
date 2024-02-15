using Microsoft.Extensions.DependencyInjection;
using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.Services.Transmission;
using NullableFox.AoXiangToDoList.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services
{
    internal class NotificationService : INotificationService
    {
        INetworkService networkService;

        public event EventHandler<SystemCollectionChangedNotificationArgs> ToDoCollectionChanged;
        public event EventHandler<PomodoroStatusChangedNotificationArgs> PomodoroStatusChanged;
        public event EventHandler<SystemCollectionChangedNotificationArgs> PomodoroRecordCollectionChanged;

        public NotificationService(INetworkService networkService)
        {
            this.networkService = networkService;
            this.networkService.NotificationReceived += NetworkService_NotificationReceived;
        }

        private void NetworkService_NotificationReceived(object sender, NotificationPacket e)
        {
            DistributeNotification(e);
        }
        void DistributeNotification(NotificationPacket e)
        {
            switch (e.NotificationType)
            {
                case NotificationType.ToDoWorkCollectionChanged:
                    SystemCollectionChangedNotificationArgs args1 = JsonHelper.ObjectFromJsonString<SystemCollectionChangedNotificationArgs>(e.Content);
                    ToDoCollectionChanged?.Invoke(this, args1);
                    break;
                case NotificationType.PomodoroStatusChanged:
                    PomodoroStatusChangedNotificationArgs args2 = JsonHelper.ObjectFromJsonString<PomodoroStatusChangedNotificationArgs>(e.Content);
                    PomodoroStatusChanged?.Invoke(this, args2);
                    break;
                case NotificationType.PomodoroRecordCollectionChanged:
                    SystemCollectionChangedNotificationArgs args3 = JsonHelper.ObjectFromJsonString<SystemCollectionChangedNotificationArgs>(e.Content);
                    PomodoroRecordCollectionChanged?.Invoke(this, args3);
                    break;
            }
        }
    }
}
