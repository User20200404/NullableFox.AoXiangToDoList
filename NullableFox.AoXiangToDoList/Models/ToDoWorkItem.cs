// Ignore Spelling: Pomodoro Nullable Xiang Ao

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Models
{
    internal class ToDoWorkItem
    {
        public int Layer { get; set; }
        public int InnerId { get; init; }
        public int ImportancePriority { get; set; }
        public int EmergencyPriority { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description {  get; set; }
        public DateTime CreateTime { get; init; }
        public DateTime StartTime { get; set; }
        public DateTime DeadLine { get; set; }
        public WorkItemStatus Status { get; set; }
        public ObservableCollection<int> SubToDoWorkItemInnerIdList { get; set; } = new ObservableCollection<int>();
        public ObservableCollection<int> PomodoroRecordInnerIdList { get; set; } = new ObservableCollection<int>();

        public override bool Equals(object obj)
        {
            if(obj is ToDoWorkItem right)
            {
                return right.InnerId == this.InnerId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return InnerId.GetHashCode();
        }

        /// <summary>
        /// TODO:进行浅拷贝，然后对可变引用类型进行深拷贝。
        /// </summary>
        /// <returns></returns>
        public ToDoWorkItem DeepCopy()
        {
            var copy = this.MemberwiseClone() as ToDoWorkItem;
            copy.SubToDoWorkItemInnerIdList = new ObservableCollection<int>(this.SubToDoWorkItemInnerIdList);
            copy.PomodoroRecordInnerIdList = new ObservableCollection<int>(this.PomodoroRecordInnerIdList);
            return copy;
        }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum WorkItemStatus
    {
        None,
        Activated,
        Expired,
        Finished
    }
}