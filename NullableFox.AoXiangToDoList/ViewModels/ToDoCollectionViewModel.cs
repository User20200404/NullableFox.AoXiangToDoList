using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using NullableFox.AoXiangToDoList.Services;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.Utilities;
using NullableFox.AoXiangToDoList.Views;
using NullableFox.AoXiangToDoList.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.ViewModels
{
    /// <summary>
    /// 待办事项集合的视图模型。
    /// </summary>
    internal partial class ToDoCollectionViewModel : MultiThreadBindableBase
    {
        IToDoWorkItemService toDoWorkService;
        INavigationService toDoWorkNavigationService;
        public ToDoCollectionViewModel(IToDoWorkItemService service)
        {
            this.toDoWorkService = service;
            this.toDoWorkNavigationService = App.Current.ServiceProvider.GetRequiredKeyedService<INavigationService>(NavigationServiceKeys.ToDoWork);
            service.ToDoCollectionChanged += Service_ToDoCollectionChangedAsync;
            ToDoWorkItemViewModels.CollectionChanged += ToDoWorkItemViewModels_CollectionChanged;
        }

        /// <summary>
        /// 将指定的<paramref name="viewModel"/>添加到相关视图模型集合中去。
        /// TODO:将指定的<paramref name="viewModel"/>添加到<see cref="ToDoWorkItemViewModels"/>，然后检查条件，再将其添加到<see cref="TodayToDoWorkItemViewModels"/>集合中去。
        /// </summary>
        async Task AddViewModelAsync(ToDoWorkItemViewModel viewModel)
        {
            //处理所有待办事项的增添
            await ToDoWorkItemViewModels.ThreadSafeAddAsync(viewModel);

            //处理今天待办事项的增添
            if (viewModel.StartTime < DateTime.Now.GetStartOfNextDay())
                await TodayToDoWorkItemViewModels.ThreadSafeAddAsync(viewModel);
        }

        /// <summary>
        /// 将指定的<paramref name="viewModel"/>从相关视图模型集合中移除。
        /// TODO:将指定的<paramref name="viewModel"/>从<see cref="ToDoWorkItemViewModels"/>移除，然后检查条件，再将其从<see cref="TodayToDoWorkItemViewModels"/>集合中移除。
        /// </summary>

        async Task RemoveViewModelAsync(ToDoWorkItemViewModel viewModel)
        {
            //处理所有待办事项的移除
            await ToDoWorkItemViewModels.ThreadSafeRemoveAsync(viewModel);

            //处理今天待办事项的移除
            await TodayToDoWorkItemViewModels.ThreadSafeRemoveAsync(viewModel);
        }

        async Task ClearViewModelsAsync()
        {
            await ToDoWorkItemViewModels.ThreadSafeClearAsync();
            await TodayToDoWorkItemViewModels.ThreadSafeClearAsync();
        }

        /// <summary>
        /// TODO:根据来自后端的集合变更通知执行相关元素的增加、修改和删除操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void Service_ToDoCollectionChangedAsync(object sender, Models.SystemCollectionChangedNotificationArgs e)
        {
            switch (e.OperationType)
            {
                case Models.CollectionOperationType.CollectionCleared: await ClearViewModelsAsync(); break; //清除操作类型，清空所有项目
                case Models.CollectionOperationType.ItemAdded:
                    await OnToDoAddedNotificationReceived(e.AddedItemInnerId.Value);
                    break;
                case Models.CollectionOperationType.ItemRemoved:
                    //移除innerId相符的元素。
                    await OnToDoRemovedNotificationReceivedAsync(e.RemovedItemInnerId.Value);
                    break;
                case Models.CollectionOperationType.ItemPropertyChanged:
                    var changedViewModel = ToDoWorkItemViewModels.FirstOrDefault(vm => vm.InnerId == e.ModifiedItemInnerId.Value);
                    await changedViewModel?.RequestUpdateAsync();
                    break;
            }
        }

        async Task OnToDoAddedNotificationReceived(int addedItemInnerId)
        {
            ToDoWorkItemViewModel existingItem = ToDoWorkItemViewModels.FirstOrDefault(vm => vm.InnerId == addedItemInnerId);
            if (existingItem != default)
            {
                //已经存在这个项目，对项目进行更新
                await existingItem.RequestUpdateAsync();
            }
            else
            {
                var toDoModel = await toDoWorkService.QueryToDoWorkItemAsync(addedItemInnerId);
                var viewModel = new ToDoWorkItemViewModel(toDoModel, toDoWorkService);
                await AddViewModelAsync(viewModel);
            }
        }

        async Task OnToDoRemovedNotificationReceivedAsync(int removedItemInnerId)
        {
            var existingItems = ToDoWorkItemViewModels.Where(vm => vm.InnerId == removedItemInnerId).ToList();
            foreach (var item in existingItems)
            {
                await RemoveViewModelAsync(item);
            }
        }

        /// <summary>
        /// TODO:对任何新添加的项目注册属性变化通知处理器，对任何移除的项目注销属性变化通知处理器。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToDoWorkItemViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is not null)
            {
                foreach (var newItem in e.NewItems)
                {
                    ToDoWorkItemViewModel vm = newItem as ToDoWorkItemViewModel;
                    vm.PropertyChanged += ToDoCollectionViewModel_PropertyChanged;
                }
            }
            if (e.OldItems is not null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    ToDoWorkItemViewModel vm = oldItem as ToDoWorkItemViewModel;
                    vm.PropertyChanged -= ToDoCollectionViewModel_PropertyChanged;
                }
            }
        }

        private void ToDoCollectionViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ToDoWorkItemViewModel viewModel = (ToDoWorkItemViewModel)sender;
            //展开时，关闭所有其他的ViewModel
            switch (e.PropertyName)
            {
                case nameof(viewModel.IsExpanded):
                    if (viewModel.IsExpanded)
                    {
                        foreach (var item in ToDoWorkItemViewModels.Where(i => i != viewModel))
                        {
                            item.PropertyChanged -= ToDoCollectionViewModel_PropertyChanged; //取消订阅事件，防止反复触发，执行完操作后恢复
                            item.IsExpanded = false;
                            item.PropertyChanged += ToDoCollectionViewModel_PropertyChanged;
                        }
                    }
                    break;
            }
        }

        [ObservableProperty]
        ObservableCollection<ToDoWorkItemViewModel> toDoWorkItemViewModels = new ObservableCollection<ToDoWorkItemViewModel>();

        [ObservableProperty]
        ObservableCollection<ToDoWorkItemViewModel> todayToDoWorkItemViewModels = new ObservableCollection<ToDoWorkItemViewModel>();

        /// <summary>
        /// 从后端拉取待办事项。
        /// </summary>
        /// <param name="clear"></param>
        /// <returns></returns>
        [RelayCommand(CanExecute = nameof(CanUpdate))]
        public async Task UpdateAsync()
        {
            CanUpdate = false;

            try
            {
                var items = await toDoWorkService.PullToDoWorkItemsAsync();
                ToDoWorkItemViewModels.Clear();
                foreach (var item in items)
                {
                    var viewModel = new ToDoWorkItemViewModel(item, toDoWorkService);
                    AddViewModelAsync(viewModel);
                }
            }
            finally { CanUpdate = true; }
        }


        /// <summary>
        /// 从后端尝试拉取待办事项。失败时通过给定的xamlRoot显示对话框。
        /// </summary>
        /// <param name="xamlRoot"></param>
        /// <returns></returns>
        [RelayCommand(CanExecute = nameof(CanUpdate))]
        public async Task TryUpdateAsync(XamlRoot xamlRoot)
        {
            try
            {
                await UpdateAsync();
            }
            catch (Exception ex)
            {
                ex.PushExceptionDialog(xamlRoot);
            }
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
        private bool canUpdate = true;

        public async Task RequestCreateNewToDo()
        {
            var todo = App.Current.ServiceProvider.GetRequiredService<ToDoWorkItemViewModel>();
            todo.Title = "新建待办事项";
            todo.Subtitle = "点击编辑按钮来自定义";
            todo.StartTime = DateTime.Now;
            todo.DeadLine = DateTime.Now.AddDays(10);
            todo.Description = "空";
            await todo.RequestCreateAsync();
        }

        [RelayCommand]
        public void RequestNavigateToEditPage(ToDoWorkItemViewModel viewModel)
        {
            toDoWorkNavigationService.NavigateToType(typeof(EditToDoWorkPage), viewModel, new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromRight
            });
        }

        [RelayCommand]
        public void RequestNavigateToListPage()
        {
            toDoWorkNavigationService.NavigateToType(typeof(AllToDoWorkPage), null, new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromLeft
            });
        }
    }
}
