using NullableFox.AoXiangToDoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Interfaces
{
    internal interface IPomodoroService
    {
        /// <summary>
        /// 请求从后端更新当前番茄钟信息。
        /// </summary>
        /// <returns></returns>
        Task<Pomodoro> RequestPullAsync();
        /// <summary>
        /// 请求将本地番茄钟信息更新到后端。
        /// </summary>
        /// <returns></returns>
        Task RequestEditAsync((int WorkTime,int RestTime) param);
        /// <summary>
        /// 请求开始番茄钟。
        /// </summary>
        /// <returns></returns>
        Task RequestStartAsync(int toDoInnerId);
        /// <summary>
        /// 请求结束番茄钟。
        /// </summary>
        /// <returns></returns>
        Task RequestStopAsync();
        /// <summary>
        /// 当番茄钟状态改变时发生。
        /// </summary>

        event EventHandler<PomodoroStatusChangedNotificationArgs> PomodoroStatusChanged;
    }
}
