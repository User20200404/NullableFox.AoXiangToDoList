using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Dispatching;
using NullableFox.AoXiangToDoList.Algorithm;
using Windows.UI;
using NullableFox.AoXiangToDoList.Utilities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList.Views.UserControls
{
    public sealed partial class TimeCounter : UserControl
    {
        private static readonly DependencyProperty StartTimeProperty = DependencyProperty.Register(nameof(StartTime), typeof(DateTime), typeof(TimeCounter), new PropertyMetadata(DateTime.Now, OnTimePropertyChanged));
        private static readonly DependencyProperty EndTimeProperty = DependencyProperty.Register(nameof(EndTime), typeof(DateTime), typeof(TimeCounter), new PropertyMetadata(DateTime.Now.AddSeconds(10), OnTimePropertyChanged));
        private static readonly DependencyProperty CountDownProperty = DependencyProperty.Register(nameof(CountDown), typeof(TimeSpan), typeof(TimeCounter), new PropertyMetadata(default(TimeSpan)));
        private static readonly DependencyProperty UpdateIntervalProperty = DependencyProperty.Register(nameof(UpdateInterval), typeof(TimeSpan), typeof(TimeCounter), new PropertyMetadata(TimeSpan.FromSeconds(1)));

        private bool loaded, tickRegistered;
        DispatcherQueueTimer timer;

        public TimeCounter()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) =>
            {
                loaded = true;
                timer?.Start();
            };
            this.Unloaded += (s, e) =>
            { 
                loaded = false;
                timer?.Stop();
            };
        }

        public DateTime StartTime
        {
            get => (DateTime)GetValue(StartTimeProperty);
            set => SetValue(StartTimeProperty, value);
        }

        public DateTime EndTime
        {
            get => (DateTime)GetValue(EndTimeProperty);
            set => SetValue(EndTimeProperty, value);
        }

        static void OnTimePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TimeCounter that = (TimeCounter)obj;
            if (args.OldValue != args.NewValue)
            {
                that.RestartCountDown();
            }
        }

        /// <summary>
        /// 获取距离EndTime剩余的时间。注意此属性并非即时。
        /// </summary>
        public TimeSpan CountDown
        {
            get => (TimeSpan)GetValue(CountDownProperty);
            private set => SetValue(CountDownProperty, value);
        }

        /// <summary>
        /// 获取或设置CountDown更新的时间间隔。
        /// </summary>
        public TimeSpan UpdateInterval
        {
            get => (TimeSpan)GetValue(UpdateIntervalProperty);
            set
            {
                SetValue(UpdateIntervalProperty, value);
                timer.Interval = value;
            }
        }

        /// <summary>
        /// 重新启动计时过程。
        /// </summary>
        private void RestartCountDown()
        {
            timer?.Stop();
            timer ??= DispatcherQueue.CreateTimer();
            timer.Interval = UpdateInterval;
            if (!tickRegistered)
            {
                timer.Tick += Timer_Tick;
                tickRegistered = true;
            }
            timer.Start();
            Update();
        }


        private void Timer_Tick(DispatcherQueueTimer sender, object args)
        {
            Update();
        }

        void Update()
        {
            string displayStr = string.Empty;
            float progress = 0f;
            DateTime now = DateTime.Now;

            if (now < StartTime)
            {
                displayStr = $"{(StartTime - now).GetCustomTimeSpanString()}后开始";
                progress = 0f;
            }
            if (now >= StartTime && now < EndTime)
            {
                displayStr = $"剩余{(EndTime - now).GetCustomTimeSpanString()}";
                progress = 1 - (float)((EndTime - now) / (EndTime - StartTime));
            }
            if (now >= EndTime)
            {
                displayStr = $"已逾期{(now - EndTime).GetCustomTimeSpanString()}";
                progress = 1f;
            }
            CountDown = (EndTime - StartTime) * (1 - progress);


            UpdateDisplay(displayStr, progress);
        }

        void UpdateDisplay(string displayStr, float progress)
        {
            countDownBlock.Text = displayStr;
            countDownProgressBar.Value = progress;
            countDownProgressBar.Foreground = Interpolation.WinUIColorBetween(Color.FromArgb(255, 0, 255, 0), Color.FromArgb(255, 255, 0, 0), progress).ToSolidBrush();
        }
    }
}
