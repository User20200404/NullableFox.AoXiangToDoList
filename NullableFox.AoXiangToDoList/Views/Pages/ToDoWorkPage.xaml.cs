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
        /// �������������Ĺ��ܣ� �������༭ҳ���ӱ༭ҳ�浼�������д���ҳ�档
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

        ObservableCollection<string> breadcrumbBarStrings = new ObservableCollection<string>() { "���д���" };

        #region �������༭ҳ��ͷ��ع���ʹ���˼�ʵ�֣���չ����ʱ�ǵ���д
        void GoToEditPage(ToDoWorkItemViewModel viewModel)
        {
            if (contentFrame.SourcePageType != typeof(EditToDoWorkPage))
                contentFrame.Navigate(typeof(EditToDoWorkPage), viewModel, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
            breadcrumbBarStrings.Add($"�༭��{viewModel.Title}({viewModel.InnerId})");
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
