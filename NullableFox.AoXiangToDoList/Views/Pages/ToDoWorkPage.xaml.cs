using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using NullableFox.AoXiangToDoList.Services;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.ViewModels;
using System;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ToDoWorkPage : Page
    {
        private INavigationService navigationService;
        public static Type Type => typeof(ToDoWorkPage);
        public static ToDoWorkPage Instance { get; private set; }
        public ToDoWorkPage()
        {
            Instance = this;
            this.InitializeComponent();
            contentFrame.Navigate(typeof(AllToDoWorkPage));

            navigationService = App.Current.ServiceProvider.GetRequiredKeyedService<INavigationService>(NavigationServiceKeys.ToDoWork);
            navigationService.NavigationRequested += NavigationService_NavigationRequested;
        }

        /// <summary>
        /// 处理待办事项导航的功能： 导航到编辑页面或从编辑页面导航回所有待办页面。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void NavigationService_NavigationRequested(object sender, PageNavigationEventArgs e)
        {
            if (e.PageType == typeof(EditToDoWorkPage))
            {
                GoToEditPage(e.Parameter as ToDoWorkItemViewModel);
            }
            else if (e.PageType == typeof(AllToDoWorkPage))
            {
                GoToListPage();
            }
        }

        ObservableCollection<string> breadcrumbBarStrings = new ObservableCollection<string>() { "所有待办" };

        #region 导航到编辑页面和返回功能使用了简单实现，扩展功能时记得重写
        void GoToEditPage(ToDoWorkItemViewModel viewModel)
        {
            if (contentFrame.SourcePageType != typeof(EditToDoWorkPage))
                contentFrame.Navigate(typeof(EditToDoWorkPage), viewModel, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
            breadcrumbBarStrings.Add($"编辑：{viewModel.Title}({viewModel.InnerId})");
        }

        void GoToListPage()
        {
            contentFrame.GoBack();
            breadcrumbBarStrings.RemoveAt(1);
        }

        #endregion
        private void BreadcrumbBar_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            contentFrame.GoBack();
            breadcrumbBarStrings.RemoveAt(1);
        }
    }
}
