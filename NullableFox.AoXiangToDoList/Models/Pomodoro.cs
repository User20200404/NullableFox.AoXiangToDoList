using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Models
{
    internal class Pomodoro
    {
        /// <summary>
        /// 番茄专注的工作时间。
        /// </summary>
        public int WorkTime;
        /// <summary>
        /// 番茄专注的休息时间。
        /// </summary>
        public int RestTime;
        public PomodoroStatus PomodoroStatus;
        public DateTime StartTime;
        public DateTime ExpectedWorkEndTime;
        public DateTime ExpectedRestEndTime;
        public int? BoundToDoWorkInnerId;
        public Pomodoro DeepCopy()
        {
            var obj = this.MemberwiseClone() as Pomodoro;
            return obj;
        }
    }

    /// <summary>
    /// 番茄钟的状态。
    /// </summary>
    internal enum PomodoroStatus
    {
        /// <summary>
        /// 不使用该值。
        /// </summary>
        None,
        /// <summary>
        /// 番茄钟尚未工作。
        /// </summary>
        NotStarted,
        /// <summary>
        /// 番茄钟正在工作。
        /// </summary>
        Working,
        /// <summary>
        /// 番茄钟专注进入休息阶段。
        /// </summary>
        Resting,
        /// <summary>
        /// 番茄钟已正常结束。
        /// </summary>
        Finished,
        /// <summary>
        /// 番茄钟被中断。
        /// </summary>
        Interrupted
    }
}
