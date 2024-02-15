using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services
{
    /// <summary>
    /// 提供了页面导航的服务。
    /// </summary>
    internal class NavigationService : INavigationService
    {
        public event EventHandler<PageNavigationEventArgs> NavigationRequested;

        public void NavigateToType(Type type, object parameter, NavigationTransitionInfo navigationTransitionInfo)
        {
            var eventArgs = new PageNavigationEventArgs(type, parameter, navigationTransitionInfo);
            NavigationRequested?.Invoke(this, eventArgs);
        }
    }

    /// <summary>
    /// 定义了页面导航服务目标的键。
    /// </summary>
    internal enum NavigationServiceKeys
    {
        Root,
        ToDoWork
    }
}
