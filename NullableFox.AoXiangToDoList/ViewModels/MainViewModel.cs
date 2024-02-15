using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.ViewModels
{
    internal class MainViewModel : MultiThreadBindableBase
    {
        public ObservableCollection<ToDoWorkItemViewModel> ToDoWorkItems { get; set; }
        public ObservableCollection<PomodoroRecordViewModel> PomodoroRecords { get; set; }
        public UserViewModel CurrentUser { get; set; }
    }
}
