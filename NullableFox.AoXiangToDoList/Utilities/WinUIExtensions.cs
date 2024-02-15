using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace NullableFox.AoXiangToDoList.Utilities
{
    internal static class WinUIExtensions
    {
        public static SolidColorBrush ToSolidBrush(this Color color)
        {
            return new SolidColorBrush(color);
        }


        public static async Task ThreadSafeRemoveAsync<T>(this ObservableCollection<T> collection, T itemToRemove)
        {
            if (collection is null) throw new NullReferenceException(nameof(collection));
            await App.Current.MainDispatcherQueue.EnqueueAsync(() => collection.Remove(itemToRemove));
        }
        public static async Task ThreadSafeAddAsync<T>(this ObservableCollection<T> collection, T itemToAdd)
        {
            if (collection is null) throw new NullReferenceException(nameof(collection));
            await App.Current.MainDispatcherQueue.EnqueueAsync(() => collection.Add(itemToAdd));
        }

        public static async Task ThreadSafeClearAsync<T>(this ObservableCollection<T> collection)
        {
            if (collection is null) throw new NullReferenceException(nameof(collection));
            await App.Current.MainDispatcherQueue.EnqueueAsync(collection.Clear);
        }
    }
}
