using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Models
{
    internal class PomodoroRecord
    {
        public  int InnerId { get; init; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
        public PomodoroRecordStatus PomodoroRecordStatus { get; set; }
    }
    internal enum PomodoroRecordStatus
    {
        None,
        Calculating,
        Finished,
        Interrupted
    }
}
