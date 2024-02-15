using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [ObservableObject]
    internal sealed partial class EditToDoWorkPage : Page
    {
        public static Type Type => typeof(EditToDoWorkPage);
        ToDoWorkItemViewModel viewModel;
        public EditToDoWorkPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            viewModel = e.Parameter as ToDoWorkItemViewModel;
            this.DataContext = viewModel;
            base.OnNavigatedTo(e);
            InitializeProperties();
        }

        /// <summary>
        /// 初始化ViewModel属性。
        /// </summary>
        void InitializeProperties()
        {
            LocalImportancePriority = viewModel.ImportancePriority;
            LocalEmergencyPriority = viewModel.EmergencyPriority;
            LocalTitle = viewModel.Title;
            LocalSubTitle = viewModel.Subtitle;
            LocalDescription = viewModel.Description;
            LocalStartDate = viewModel.StartTime.GetParitalDateTimeOffset();
            LocalEndDate = viewModel.DeadLine.GetParitalDateTimeOffset();
            LocalStartTime = viewModel.StartTime.GetPartialTimeSpan();
            LocalEndTime = viewModel.DeadLine.GetPartialTimeSpan();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task CompleteEditAsync()
        {
            try
            {
                await RequestModifyAsync();
                GoBackToListPage();
            }
            catch (Exception ex)
            {
                ex.PushExceptionDialog(XamlRoot);
            }
        }

        /// <summary>
        /// 请求导航到待办事项列表页面。
        /// </summary>
        [RelayCommand]
        void GoBackToListPage()
        {
            App.Current.ServiceProvider.GetRequiredService<ToDoCollectionViewModel>().RequestNavigateToListPage();
        }

        /// <summary>
        /// 请求提交待办事项修改。
        /// </summary>
        /// <returns>产生的Task任务实例。</returns>
        [RelayCommand]
        async Task RequestModifyAsync()
        {
            ToDoWorkItemViewModel tempViewModel = viewModel.DeepCopy();
            tempViewModel.Title = LocalTitle;
            tempViewModel.Subtitle = LocalSubTitle;
            tempViewModel.Description = LocalDescription;
            tempViewModel.ImportancePriority = LocalImportancePriority;
            tempViewModel.EmergencyPriority = LocalEmergencyPriority;
            tempViewModel.StartTime = TimeHelper.Combine(LocalStartDate, LocalStartTime);
            tempViewModel.DeadLine = TimeHelper.Combine(LocalEndDate, LocalEndTime);
            await tempViewModel.RequestModifyAsync();
        }

        [ObservableProperty]
        double canvasCoordinateX;
        [ObservableProperty]
        double canvasCoordinateY;
        [ObservableProperty]
        int localImportancePriority;
        [ObservableProperty]
        int localEmergencyPriority;
        [ObservableProperty]
        DateTimeOffset localStartDate;
        [ObservableProperty]
        DateTimeOffset localEndDate;
        [ObservableProperty]
        TimeSpan localStartTime;
        [ObservableProperty]
        TimeSpan localEndTime;
        [ObservableProperty]
        string localTitle;
        [ObservableProperty]
        string localSubTitle;
        [ObservableProperty]
        string localDescription;
        /// <summary>
        /// TODO:根据Canvas内的控件坐标计算两个Slider的值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void PriorityGrid_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!selectPriorityToggleButton.IsChecked.Value) return;

            var rawPoint = e.GetCurrentPoint(sliderCanvas);
            double rawX = rawPoint.Position.X;
            double rawY = rawPoint.Position.Y;
            double wDiff = sliderCanvas.ActualWidth - horizontalSlider.ActualWidth + 12;
            double hDiff = sliderCanvas.ActualHeight - verticalSlider.ActualHeight + 12; //12是Slider滑块的默认宽高

            //计算滑块区域的四边界坐标
            double left = wDiff / 2;
            double right = sliderCanvas.ActualWidth - left;
            double top = hDiff / 2;
            double bottom = sliderCanvas.ActualHeight - top;

            double cx = rawX, cy = rawY;
            if (cx < left) cx = left;
            if (cx > right) cx = right;
            if (cy < top) cy = top;
            if (cy > bottom) cy = bottom;
            CanvasCoordinateX = cx;
            CanvasCoordinateY = cy;

            double hv = ((cx - left) / (right - left)) * horizontalSlider.Maximum;
            double vv = (1 - (cy - top) / (bottom - top)) * verticalSlider.Maximum;
            LocalImportancePriority = (int)Math.Round(hv, 0);
            LocalEmergencyPriority = (int)Math.Round(vv, 0);
        }

        private void PriorityGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            selectPriorityToggleButton.IsChecked = false;
        }
    }
}
