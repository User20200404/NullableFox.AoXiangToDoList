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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList.Views.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        AppConfigurationViewModel AppConfigurationViewModel => App.Current.ServiceProvider.GetRequiredService<AppConfigurationViewModel>();
        ApplicationViewModel AppViewModel => App.Current.ServiceProvider.GetRequiredService<ApplicationViewModel>();
        public MainWindow()
        {
            AppViewModel.GoToMiniFocusViewRequested += AppViewModel_GoToMiniFocusViewRequested;
            AppViewModel.GoToNormalViewRequested += AppViewModel_GoToNormalViewRequested;

            App.Current.MainDispatcherQueue = this.DispatcherQueue;
            AppConfigurationViewModel.LoadAsync().GetAwaiter().GetResult();
            var windowSize = AppConfigurationViewModel.ApplicationMainWindowSize;
            this.Width = windowSize.Width;
            this.Height = windowSize.Height;
            this.WindowState = AppConfigurationViewModel.ApplicationMainWindowState;

            this.InitializeComponent();
            this.Closed += MainWindow_Closed;
            this.ExtendsContentIntoTitleBar = true;
            this.Title = "°¿ÏèÇåµ¥ 1.0.0.0";
        }

        private void AppViewModel_GoToNormalViewRequested(object sender, EventArgs e)
        {
            this.Show();
            App.Current.ServiceProvider.GetRequiredService<FocusViewWindow>().Hide();
        }

        private void AppViewModel_GoToMiniFocusViewRequested(object sender, EventArgs e)
        {
            this.Hide();
            App.Current.ServiceProvider.GetRequiredService<FocusViewWindow>().Show();
        }

        private async void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            AppConfigurationViewModel.ApplicationMainWindowState = this.WindowState;
            this.WindowState = WindowState.Normal;
            AppConfigurationViewModel.ApplicationMainWindowSize =
                new((int)Width, (int)Height);
            await AppConfigurationViewModel.SaveAsync();

            AppViewModel.ExitUI();
        }
    }
}
