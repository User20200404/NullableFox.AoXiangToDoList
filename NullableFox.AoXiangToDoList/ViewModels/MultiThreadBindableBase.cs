using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.ViewModels
{
    /// <summary>
    /// 为所有ViewModel提供可跨线程的通知功能。
    /// </summary>
    internal class MultiThreadBindableBase : ObservableObject
    {
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            bool hasThreadAccess = App.Current.MainDispatcherQueue.HasThreadAccess;
            if (hasThreadAccess)
                base.OnPropertyChanged(e);
            else App.Current.MainDispatcherQueue.TryEnqueue(() => base.OnPropertyChanged(e));
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            bool hasThreadAccess = App.Current.MainDispatcherQueue.HasThreadAccess;
            if (hasThreadAccess)
                base.OnPropertyChanging(e);
            else App.Current.MainDispatcherQueue.TryEnqueue(() => base.OnPropertyChanging(e));
        }
    }
}
