using NullableFox.AoXiangToDoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Interfaces
{
    /// <summary>
    /// 通知事件接口。
    /// </summary>
    internal interface INotificationService
    {
        public event EventHandler<SystemCollectionChangedNotificationArgs> ToDoCollectionChanged;
        public event EventHandler<PomodoroStatusChangedNotificationArgs> PomodoroStatusChanged;
        public event EventHandler<SystemCollectionChangedNotificationArgs> PomodoroRecordCollectionChanged;
    }
}
