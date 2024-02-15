using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Transmission
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum RequestType
    {
        None,
        EnumerateToDoWorkList,
        QueryToDoWork,
        CreateToDoWork,
        EditToDoWork,
        DeleteToDoWork,
        EditPomodoro,
        StartPomodoro,
        GetPomodoro,
        EndPomodoro,
        UserRegister,
        UserLogin,
        UserLogout,
        Synchronize,
        GetCurrentUser,
        ExitApplication,
        ModifyUserInfo,
        RegisterNotificationContext,
        UnregisterNotificationContext,
        GetNotification,
        TrapConnectionForNotifications,
        SaveSystemData,
        QueryPomodoroRecord,
        EnumeratePomodoroRecords,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum ResponseStatus
    {
        None,
        Success,
        Warning,
        Failure
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum NotificationType
    {
        /// <summary>
        /// 表示待办事项集合的元素添加、移除、清空或属性被修改
        /// </summary>
        ToDoWorkCollectionChanged,
        PomodoroRecordCollectionChanged,
        PomodoroStatusChanged
    }

}
