using Microsoft.UI.Xaml.Media;
using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.ViewModels
{
    internal class PomodoroRecordViewModel : MultiThreadBindableBase
    {
        PomodoroRecord pomodoroRecord;
        IPomodoroRecordService pomodoroRecordService;
        public PomodoroRecordViewModel(PomodoroRecord pomodoroRecord, IPomodoroRecordService recordService)
        {
            this.pomodoroRecord = pomodoroRecord;
            this.pomodoroRecordService = recordService;
        }

        public PomodoroRecordViewModel(IPomodoroRecordService service) : this(new(), service) { }

        public int InnerId => pomodoroRecord.InnerId;

        public DateTime StartTime
        {
            get => pomodoroRecord.StartTime;
            set
            {
                if (pomodoroRecord.StartTime != value)
                {
                    pomodoroRecord.StartTime = value;
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }

        public DateTime EndTime
        {
            get => pomodoroRecord.EndTime;
            set
            {
                if (pomodoroRecord.EndTime != value)
                {
                    pomodoroRecord.EndTime = value;
                    OnPropertyChanged(nameof(EndTime));
                }
            }
        }

        public TimeSpan Duration => pomodoroRecord.Duration;

        public PomodoroRecordStatus PomodoroRecordStatus
        {
            get => pomodoroRecord.PomodoroRecordStatus;
            set
            {
                if (pomodoroRecord.PomodoroRecordStatus != value)
                {
                    pomodoroRecord.PomodoroRecordStatus = value;
                    OnPropertyChanged(nameof(PomodoroRecordStatus));
                    OnPropertyChanged(nameof(StatusForeground));
                }
            }
        }

        public SolidColorBrush StatusForeground =>
            new SolidColorBrush(PomodoroRecordStatus == PomodoroRecordStatus.Finished ?
            Windows.UI.Color.FromArgb(255, 0, 255, 0) :
            Windows.UI.Color.FromArgb(255, 255, 0, 0));


        /// <summary>
        /// 请求从后端拉取数据并更新本番茄钟记录对象。
        /// </summary>
        /// <returns></returns>
        public async Task RequestUpdateAsync()
        {
            var newRecord = await pomodoroRecordService.QueryPomodoroRecord(this.InnerId);
            this.StartTime = newRecord.StartTime;
            this.EndTime = newRecord.EndTime;
            this.PomodoroRecordStatus = newRecord.PomodoroRecordStatus;
        }
    }

}
