using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Models
{
    internal class PomodoroRecord
    {
        public int InnerId;
        public DateTime StartTime;
        public DateTime EndTime;
        public TimeSpan Duration => EndTime - StartTime;
        public PomodoroRecordStatus PomodoroRecordStatus;
    }
    internal enum PomodoroRecordStatus
    {
        None,
        Calculating,
        Finished,
        Interrupted
    }
}
