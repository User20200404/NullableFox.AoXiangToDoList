using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NullableFox.AoXiangToDoList.Services;
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

namespace NullableFox.AoXiangToDoList.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PomodoroPage : Page
    {
        public static Type Type => typeof(PomodoroPage);
        PomodoroViewModel PomodoroViewModel => App.Current.ServiceProvider.GetRequiredService<PomodoroViewModel>();
        ToDoCollectionViewModel ToDoCollection => App.Current.ServiceProvider.GetRequiredService<ToDoCollectionViewModel>();
        PomodoroRecordCollectionViewModel PomodoroRecordCollection => App.Current.ServiceProvider.GetRequiredService<PomodoroRecordCollectionViewModel>();
        public PomodoroPage()
        {
            this.Loaded += PomodoroPage_Loaded;
            this.InitializeComponent();
            PomodoroRecordCollection.UpdateAsync();
        }

        private void PomodoroPage_Loaded(object sender, RoutedEventArgs e)
        {
            _ = PomodoroViewModel.RequestUpdateAsync();
        }

        bool IsDurationRecommended(double workTime, double restTime)
        {
            return workTime >= 20 && restTime <= 25
                && workTime / restTime >= 3.5;
        }

        double FullOpacityIfDurationRecommended(double workTime, double restTime)
        {
            return IsDurationRecommended(workTime, restTime) ? 1d : 0d;
        }

        string GetTimeSpanString(TimeSpan span)
        {
            return TimeHelper.GetClockStyleTimeSpanString(span);
        }
    }
}
