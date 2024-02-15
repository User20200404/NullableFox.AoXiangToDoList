using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NullableFox.AoXiangToDoList.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList.Views.UserControls
{
    internal sealed partial class PomodoroOptionSelector : UserControl
    {
        PomodoroViewModel PomodoroViewModel => App.Current.ServiceProvider.GetRequiredService<PomodoroViewModel>();
        ToDoCollectionViewModel ToDoCollection => App.Current.ServiceProvider.GetRequiredService<ToDoCollectionViewModel>();
        public PomodoroOptionSelector()
        {
            this.InitializeComponent();
        }
    }
}
