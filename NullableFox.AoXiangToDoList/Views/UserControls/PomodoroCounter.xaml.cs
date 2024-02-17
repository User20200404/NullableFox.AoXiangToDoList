using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NullableFox.AoXiangToDoList.Utilities;
using NullableFox.AoXiangToDoList.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList.Views.UserControls
{
    internal sealed partial class PomodoroCounter : UserControl
    {
        public ApplicationViewModel AppViewModel => App.Current.ServiceProvider.GetRequiredService<ApplicationViewModel>();
        static DependencyProperty IsMiniViewProperty = DependencyProperty.Register(nameof(IsMiniView), typeof(bool), typeof(PomodoroCounter), new PropertyMetadata(false));

        public bool IsMiniView
        {
            get => (bool)GetValue(IsMiniViewProperty);
            set => SetValue(IsMiniViewProperty, value);
        }
        PomodoroViewModel PomodoroViewModel => App.Current.ServiceProvider.GetRequiredService<PomodoroViewModel>();
        public PomodoroCounter()
        {
            this.InitializeComponent();
            PomodoroViewModel.PropertyChanged += PomodoroViewModel_PropertyChanged;
        }

        private void PomodoroViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName) 
            {
                case nameof(PomodoroViewModel.IsActivated):
                    if(!PomodoroViewModel.IsActivated)
                    {
                        AppViewModel.GoToNormalView();
                    }
                    break;
            }
        }

        string GetTimeSpanString(TimeSpan span)
        {
            return TimeHelper.GetClockStyleTimeSpanString(span);
        }
    }
}
