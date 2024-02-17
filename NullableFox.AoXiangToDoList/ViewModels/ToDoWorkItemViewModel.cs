using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.Views;
using NullableFox.AoXiangToDoList.Views.Pages;
using NullableFox.AoXiangToDoList.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.ViewModels
{
    /// <summary>
    /// 待办事项视图模型。
    /// </summary>
    internal partial class ToDoWorkItemViewModel : MultiThreadBindableBase
    {
        IToDoWorkItemService service;
        ToDoWorkItem toDoWorkItem;
        PomodoroViewModel pomodoroViewModel;
        INavigationService rootNavigationService;
        INavigationService toDoWorkNavigationService;
        public ToDoWorkItemViewModel(IToDoWorkItemService service) : this(new ToDoWorkItem(), service) { }

        public ToDoWorkItemViewModel(ToDoWorkItem baseToDoWorkItem, IToDoWorkItemService service)
        {
            toDoWorkItem = baseToDoWorkItem;
            this.service = service;
            this.pomodoroViewModel = App.Current.ServiceProvider.GetRequiredService<PomodoroViewModel>();
            this.rootNavigationService = App.Current.ServiceProvider.GetRequiredKeyedService<INavigationService>(NavigationServiceKeys.Root);
            this.toDoWorkNavigationService = App.Current.ServiceProvider.GetRequiredKeyedService<INavigationService>(NavigationServiceKeys.ToDoWork);
        }

        /// <summary>
        /// 创建本对象的深拷贝。
        /// </summary>
        /// <returns>新创建的对象。</returns>
        public ToDoWorkItemViewModel DeepCopy()
        {
            var copy = this.MemberwiseClone() as ToDoWorkItemViewModel;
            copy.toDoWorkItem = toDoWorkItem.DeepCopy();
            return copy;
        }

        #region Property Wrappers
        public int Layer
        {
            get => toDoWorkItem.Layer;
            set => SetProperty(ref toDoWorkItem.Layer, value);
        }

        [JsonInclude]
        public int InnerId => toDoWorkItem.InnerId;

        public int ImportancePriority
        {
            get => toDoWorkItem.ImportancePriority;
            set => SetProperty(ref toDoWorkItem.ImportancePriority, value);
        }

        public int EmergencyPriority
        {
            get => toDoWorkItem.EmergencyPriority;
            set => SetProperty(ref toDoWorkItem.EmergencyPriority, value);
        }

        public string Title
        {
            get => toDoWorkItem.Title;
            set => SetProperty(ref toDoWorkItem.Title, value);
        }

        public string Subtitle
        {
            get => toDoWorkItem.Subtitle;
            set => SetProperty(ref toDoWorkItem.Subtitle, value);
        }

        public string Description
        {
            get => toDoWorkItem.Description;
            set => SetProperty(ref toDoWorkItem.Description, value);
        }
        [JsonInclude]
        public DateTime CreateTime => toDoWorkItem.CreateTime;

        public DateTime StartTime
        {
            get => toDoWorkItem.StartTime;
            set => SetProperty(ref toDoWorkItem.StartTime, value);
        }

        public DateTime DeadLine
        {
            get => toDoWorkItem.DeadLine;
            set => SetProperty(ref toDoWorkItem.DeadLine, value);
        }

        public WorkItemStatus Status
        {
            get => toDoWorkItem.Status;
            set => SetProperty(ref toDoWorkItem.Status, value);
        }

        public ObservableCollection<int> SubToDoWorkItemInnerIdList
        {
            get => toDoWorkItem.SubToDoWorkItemInnerIdList;
            set => SetProperty(ref toDoWorkItem.SubToDoWorkItemInnerIdList, value);
        }

        public ObservableCollection<int> PomodoroRecordInnerIdList
        {
            get => toDoWorkItem.PomodoroRecordInnerIdList;
            set => SetProperty(ref toDoWorkItem.PomodoroRecordInnerIdList, value);
        }
        #endregion

        #region Additive Properties
        [ObservableProperty]
        bool isExpanded;
        [ObservableProperty]
        bool isHovering;

        public ToDoWorkItem InnerModel => toDoWorkItem;
        #endregion

        [RelayCommand]
        public async Task<int> RequestCreateAsync()
        {
            return await service.CreateToDoWorkAsync(toDoWorkItem);
        }

        [RelayCommand]
        public async Task RequestModifyAsync()
        {
            await service.EditToDoWorkItemAsync(toDoWorkItem);
        }
        [RelayCommand]
        public async Task RequestDeleteAsync()
        {
            await service.DeleteToDoWorkItemAsync(this.InnerId);
        }


        /// <summary>
        /// 请求从后端拉取并跟新本待办事项信息。
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task RequestUpdateAsync()
        {
            var item = await service.QueryToDoWorkItemAsync(this.InnerId);
            this.Subtitle = item.Subtitle;
            this.StartTime = item.StartTime;
            this.DeadLine = item.DeadLine;
            this.PomodoroRecordInnerIdList = item.PomodoroRecordInnerIdList;
            this.SubToDoWorkItemInnerIdList = item.SubToDoWorkItemInnerIdList;
            this.Description = item.Description;
            this.Title = item.Title;
            this.Status = item.Status;
            this.EmergencyPriority = item.EmergencyPriority;
            this.ImportancePriority = item.ImportancePriority;
            this.Layer = item.Layer;
        }

        [RelayCommand]
        public void NavigateToEditPage()
        {
            toDoWorkNavigationService.NavigateToType(typeof(EditToDoWorkPage), this);
        }

        [RelayCommand]
        public void NavigateToListPage()
        {
            toDoWorkNavigationService.NavigateToType(typeof(AllToDoWorkPage));
        }

        [RelayCommand]
        public void NavigateToPomodoroPage()
        {
            pomodoroViewModel.CurrentSelection = this;
            rootNavigationService.NavigateToType(typeof(PomodoroPage));
        }
    }

}
