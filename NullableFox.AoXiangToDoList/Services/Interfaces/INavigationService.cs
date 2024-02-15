using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Interfaces
{
    internal interface INavigationService
    {
        /// <summary>
        /// 请求导航到某个页面。
        /// </summary>
        /// <param name="type"></param>
        void NavigateToType(Type type) => NavigateToType(type, null);

        /// <summary>
        /// 请求导航到某个页面并附带请求参数。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parameter"></param>
        void NavigateToType(Type type, object parameter) => NavigateToType(type, parameter, null);

        void NavigateToType(Type type, object parameter, NavigationTransitionInfo navigationTransitionInfo);

        /// <summary>
        /// 当请求导航时发生该事件。
        /// </summary>
        event EventHandler<PageNavigationEventArgs> NavigationRequested;
    }

    internal record PageNavigationEventArgs(Type PageType, object Parameter, NavigationTransitionInfo TransitionInfo);
}
