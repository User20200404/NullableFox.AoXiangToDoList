using NullableFox.AoXiangToDoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Interfaces
{
    internal interface IPomodoroRecordService
    {
        /// <summary>
        /// 从后端请求拉取番茄钟记录列表。
        /// </summary>
        /// <returns>番茄钟记录列表。</returns>
        Task<List<PomodoroRecord>> PullPomodoroRecordsAsync();
        /// <summary>
        /// 查询指定innerId的番茄钟记录。
        /// </summary>
        /// <param name="innerId"></param>
        /// <returns></returns>
        Task<PomodoroRecord> QueryPomodoroRecord(int innerId);
        /// <summary>
        /// 当接收到番茄钟记录集合变化通知时转发。
        /// </summary>
        public event EventHandler<SystemCollectionChangedNotificationArgs> PomodoroRecordCollectionChanged;
    }
}
