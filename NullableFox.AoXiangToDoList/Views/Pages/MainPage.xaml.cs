using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using NullableFox.AoXiangToDoList.Services;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public sealed partial class MainPage : Page
    {
        INavigationService NavigationService => App.Current.ServiceProvider.GetRequiredKeyedService<INavigationService>(NavigationServiceKeys.Root);
        public MainPage()
        {
            this.InitializeComponent();
            NavigationService.NavigationRequested += NavigationService_NavigationRequested;
            contentFrame.Navigating += ContentFrame_Navigating;
        }

        /// <summary>
        /// TODO: 当contentFrame导航到某个页面时，判断该页面对应的是哪个NavigationViewItem，然后选中该NavigationViewItem。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var navItems = navigationView.MenuItems.Select(i => i as NavigationViewItem).Union(
                navigationView.FooterMenuItems.Select(i => i as NavigationViewItem)
                );
            foreach (var item in navItems)
            {
                if(item.Tag as Type == e.SourcePageType)
                {
                    navigationView.SelectedItem = item;
                }
            }
        }

        private void NavigationService_NavigationRequested(object sender, PageNavigationEventArgs e)
        {
            if (e.PageType.IsAssignableTo(typeof(Page)))
            {
                Navigate(e.PageType);
            }
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var pageType = args.InvokedItemContainer.Tag as Type;
            if (pageType.IsAssignableTo(typeof(Page)))
                Navigate(pageType);
        }

        void Navigate(Type pageType)
        {
            if (contentFrame.SourcePageType != pageType)
                this.contentFrame.Navigate(pageType, null, new DrillInNavigationTransitionInfo());
        }
    }
}
