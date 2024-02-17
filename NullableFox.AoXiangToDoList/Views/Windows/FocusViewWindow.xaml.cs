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
using Windows.UI.ViewManagement;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList.Views.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class FocusViewWindow : WindowEx
    {
        public FocusViewWindow(ApplicationViewModel appViewModel)
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            this.IsMinimizable = false;
            this.IsAlwaysOnTop = true;
            this.IsMaximizable = false;
            this.IsResizable = false;

            this.Closed += FocusViewWindow_Closed;
            appViewModel.ExitUIRequested += AppViewModel_ExitUIRequested;
        }

        private void AppViewModel_ExitUIRequested(object sender, EventArgs e)
        {
            this.Closed -= FocusViewWindow_Closed;
            this.Close();
        }

        private void FocusViewWindow_Closed(object sender, WindowEventArgs args)
        {
            App.Current.ServiceProvider.GetRequiredService<ApplicationViewModel>().GoToNormalView();
            this.Hide();
            args.Handled = true;
        }
    }
}
