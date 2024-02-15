using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NullableFox.AoXiangToDoList.Exceptions;
using NullableFox.AoXiangToDoList.ViewModels;
using System;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NullableFox.AoXiangToDoList.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [ObservableObject]
    public sealed partial class AllToDoWorkPage : Page
    {
        public static Type Type => typeof(AllToDoWorkPage);
        private ToDoCollectionViewModel viewModel;
        private int ToDoListViewSelectedCount => toDoListView.SelectedItems.Count;
        private int ToDoListViewItemsCount => toDoListView.Items.Count;
        public AllToDoWorkPage()
        {
            viewModel = App.Current.ServiceProvider.GetRequiredService<ToDoCollectionViewModel>();
            this.DataContext = viewModel;
            this.InitializeComponent();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await viewModel.UpdateAsync();
            }
            catch (Exception ex)
            {
                ex.PushExceptionDialog(this.XamlRoot);
            }
        }

        public static string PriorityToString(int priority)
        {
            return $"{priority switch
            {
                <= 3 => "低",
                > 3 and <= 6 => "一般",
                > 6 and <= 9 => "高",
                >= 10 => "非常高"
            }}({priority})";
        }

        ListViewSelectionMode BooleanToListViewSelectionMode(bool b)
        {
            return b ? ListViewSelectionMode.Multiple : ListViewSelectionMode.None;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await viewModel.RequestCreateNewToDo();
            }
            catch (Exception ex) { ex.PushExceptionDialog(XamlRoot); }
        }

        private async void MultiDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int errCount = 0;
            ApplicationShowableException exception = null;
            try
            {
                foreach (var item in toDoListView.SelectedItems.ToList())
                {
                    ToDoWorkItemViewModel vm = (ToDoWorkItemViewModel)item;
                    await vm.RequestDeleteAsync();
                }
            }
            catch (ApplicationShowableException ex)
            {
                errCount++;
                exception = ex;
            }
            if (exception is not null)
                await new ApplicationShowableException(exception) { Description = $"{exception.Description}\n还有{errCount}条此类错误。" }.ShowByDialogAsync(XamlRoot);
            selectToggleButton.IsChecked = false;
        }

        private void ToDoListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ToDoListViewSelectedCount));
        }

        /// <summary>
        /// TODO: 将ViewModel中IsHovering属性设置为true。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemExpander_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ToDoWorkItemViewModel todo = (sender as Expander).DataContext as ToDoWorkItemViewModel;
            todo.IsHovering = true;
        }

        private void ItemExpander_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ToDoWorkItemViewModel todo = (sender as Expander).DataContext as ToDoWorkItemViewModel;
            todo.IsHovering = false;
        }
    }
}
