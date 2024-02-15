using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.Views;
using System;
using System.Threading.Tasks;
using System.Timers;
namespace NullableFox.AoXiangToDoList.ViewModels
{
    /// <summary>
    /// 番茄钟的视图模型。
    /// </summary>
    internal partial class PomodoroViewModel : MultiThreadBindableBase
    {
        private IPomodoroService pomodoroService;
        private Pomodoro model;
        public PomodoroViewModel(Pomodoro model, IPomodoroService pomodoroService, INetworkService networkService)
        {
            this.pomodoroService = pomodoroService;
            this.model = model;
            timer.Interval = 1000;
            timer.Elapsed += (s, e) => UpdateDisplay();
            pomodoroService.PomodoroStatusChanged += PomodoroService_PomodoroStatusChanged;
            networkService.NetworkReconnected += PomodoroService_PomodoroServiceReconnected;
            _ = RequestUpdateAsync();
        }

        /// <summary>
        /// TODO: 更新番茄钟数据和计时器状态。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void PomodoroService_PomodoroServiceReconnected(object sender, EventArgs e)
        {
            RequestUpdateAsync().GetAwaiter().GetResult();
        }

        private Timer timer = new Timer();
        private void PomodoroService_PomodoroStatusChanged(object sender, PomodoroStatusChangedNotificationArgs e)
        {
            RequestUpdateAsync().GetAwaiter().GetResult();
        }
        #region Property Wrappers
        /// <summary>
        /// 获取或设置番茄专注的工作时长（分钟）。
        /// </summary>
        public int WorkTime
        {
            get => model.WorkTime;
            set
            {
                if (model.WorkTime != value)
                {
                    model.WorkTime = value;
                    OnPropertyChanged(nameof(WorkTime));
                    OnPropertyChanged(nameof(IsPlanRecommended));
                }
            }
        }

        /// <summary>
        /// 获取或设置番茄钟的休息时间（分钟）。
        /// </summary>
        public int RestTime
        {
            get => model.RestTime;
            set
            {
                if (model.RestTime != value)
                {
                    model.RestTime = value;
                    OnPropertyChanged(nameof(RestTime));
                    OnPropertyChanged(nameof(IsPlanRecommended));
                }
            }
        }
        /// <summary>
        /// 当前番茄钟的工作状态。
        /// </summary>
        public PomodoroStatus PomodoroStatus
        {
            get => model.PomodoroStatus;
            set
            {
                if (model.PomodoroStatus != value)
                {
                    model.PomodoroStatus = value;
                    OnPropertyChanged(nameof(PomodoroStatus));
                    OnPropertyChanged(nameof(IsWorking));
                    OnPropertyChanged(nameof(IsResting));
                    OnPropertyChanged(nameof(IsActivated));
                }
            }
        }

        public DateTime StartTime
        {
            get => model.StartTime;
            set
            {
                if (model.StartTime != value)
                {
                    model.StartTime = value;
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }

        public DateTime ExpectedWorkEndTime
        {
            get => model.ExpectedWorkEndTime;
            set
            {
                if (model.ExpectedWorkEndTime != value)
                {
                    model.ExpectedWorkEndTime = value;
                    OnPropertyChanged(nameof(ExpectedWorkEndTime));
                }
            }
        }
        public DateTime ExpectedRestEndTime
        {
            get => model.ExpectedRestEndTime;
            set
            {
                if (model.ExpectedRestEndTime != value)
                {
                    model.ExpectedRestEndTime = value;
                    OnPropertyChanged(nameof(ExpectedRestEndTime));
                }
            }
        }
        public int? BoundToDoWorkInnerId
        {
            get => model.BoundToDoWorkInnerId;
            set
            {
                if (model.BoundToDoWorkInnerId != value)
                {
                    model.BoundToDoWorkInnerId = value;
                    OnPropertyChanged(nameof(BoundToDoWorkInnerId));
                }
            }
        }
        #endregion

        #region Additive Properties
        /// <summary>
        /// 获取一个值，指示了当前番茄钟是否正在专注阶段。
        /// </summary>
        public bool IsWorking => model.PomodoroStatus == PomodoroStatus.Working;

        /// <summary>
        /// 获取一个值，指示了当前番茄钟是否已被激活（工作或休息阶段）
        /// </summary>
        public bool IsActivated => IsWorking || IsResting;

        /// <summary>
        /// 获取一个值，指示了当前番茄钟是否正在休息阶段。
        /// </summary>
        public bool IsResting => model.PomodoroStatus == PomodoroStatus.Resting;
        [ObservableProperty]
        private int currentStage;
        [ObservableProperty]
        private ToDoWorkItemViewModel currentSelection;
        [ObservableProperty]
        private TimeSpan timeSpanBeforeNextSection;
        [ObservableProperty]
        /// <summary>
        /// 指示了当前番茄钟的总进度。
        /// </summary>
        private double totalProgress;

        /// <summary>
        /// 指示了当前的时间规划是否合理。
        /// </summary>
        public bool IsPlanRecommended => WorkTime >= 20 && WorkTime <= 60 && RestTime >= 3 && RestTime <= 25
            && WorkTime / (float)RestTime >= 3.5f;
        #endregion


        /// <summary>
        /// 更新进度显示。
        /// </summary>
        void UpdateProgress()
        {
            double ttProgress = 1 - (ExpectedRestEndTime - DateTime.Now) / (ExpectedRestEndTime - StartTime);
            TotalProgress = double.Clamp(ttProgress, 0, 1);
        }

        /// <summary>
        /// 更新距离下一个阶段的剩余时间显示。
        /// </summary>
        void UpdateTimeSpanBeforeNextSection()
        {
            TimeSpanBeforeNextSection = IsWorking ?
                ExpectedWorkEndTime - DateTime.Now :
                ExpectedRestEndTime - DateTime.Now;
        }

        /// <summary>
        /// 更新需要显示的内容，这包括进度和剩余时间信息。
        /// </summary>
        void UpdateDisplay()
        {
            UpdateProgress();
            UpdateTimeSpanBeforeNextSection();
        }

        /// <summary>
        /// 更新计时器的状态（启动、停止等）。
        /// </summary>
        void UpdateTimerStatus()
        {
            switch (this.PomodoroStatus)
            {
                case PomodoroStatus.Working:
                    timer.Start();
                    CurrentStage = 1;
                    break;
                case PomodoroStatus.Resting:
                    timer.Start();
                    CurrentStage = 2;
                    break;
                case PomodoroStatus.Finished:
                case PomodoroStatus.Interrupted:
                    timer.Stop();
                    CurrentStage = 0;
                    break;
            }
        }

        #region Commands
        /// <summary>
        /// 请求从服务拉取最新的番茄钟状态。
        /// </summary>
        /// <returns></returns>
        public async Task RequestUpdateAsync()
        {
            var pomodoro = await pomodoroService.RequestPullAsync();
            this.PomodoroStatus = pomodoro.PomodoroStatus;
            this.WorkTime = pomodoro.WorkTime;
            this.RestTime = pomodoro.RestTime;
            this.StartTime = pomodoro.StartTime;
            this.ExpectedRestEndTime = pomodoro.ExpectedRestEndTime;
            this.ExpectedWorkEndTime = pomodoro.ExpectedWorkEndTime;
            this.BoundToDoWorkInnerId = pomodoro.BoundToDoWorkInnerId;

            UpdateDisplay(); //手动更新一次显示状态，否则需要等到Timer在1000ms后激活才能进行第一次更新。
            UpdateTimerStatus();
        }
        [RelayCommand]
        public async Task RequestEditAsync()
        {
            await pomodoroService.RequestEditAsync((WorkTime, RestTime));
        }
        [RelayCommand]
        public async Task RequestStartAsync()
        {
            await pomodoroService.RequestStartAsync(CurrentSelection.InnerId);
        }

        public async Task RequestStopAsync()
        {
            await pomodoroService.RequestStopAsync();
        }
        [RelayCommand]
        public async Task RequestEditAndStartAsync()
        {
            await RequestEditAsync();
            await RequestStartAsync();
        }
        [RelayCommand]
        public async Task TryRequestStopAsync(XamlRoot xamlRoot)
        {
            try
            {
                await RequestStopAsync();
            }
            catch (Exception ex)
            {
                ex.PushExceptionDialog(xamlRoot);
            }
        }

        [RelayCommand]
        public async Task TryRequestEditAndStartAsync(XamlRoot xamlRoot)
        {
            try
            {
                await RequestEditAndStartAsync();
            }
            catch (Exception ex)
            {
                ex.PushExceptionDialog(xamlRoot);
            }
        }
        [RelayCommand]
        public async Task TryTogglePomodoroStatus(XamlRoot xamlRoot)
        {
            try
            {
                await TogglePomodoroStatus();
            }
            catch (Exception ex)
            {
                ex.PushExceptionDialog(xamlRoot);
            }
        }
        [RelayCommand]
        public async Task TogglePomodoroStatus()
        {
            if (IsWorking || IsResting)
            {
                await RequestStopAsync();
            }
            else
            {
                await RequestEditAndStartAsync();
            }
        }
        #endregion

        public PomodoroViewModel(IPomodoroService service, INetworkService networkService) : this(new(), service, networkService) { }
    }
}
