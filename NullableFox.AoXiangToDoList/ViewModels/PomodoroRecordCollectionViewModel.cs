using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.Utilities;
using NullableFox.AoXiangToDoList.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.ViewModels
{
    internal partial class PomodoroRecordCollectionViewModel : MultiThreadBindableBase
    {
        private IPomodoroRecordService pomodoroRecordService;

        public PomodoroRecordCollectionViewModel(IPomodoroRecordService pomodoroRecordService)
        {
            this.pomodoroRecordService = pomodoroRecordService;
            pomodoroRecordService.PomodoroRecordCollectionChanged += PomodoroRecordService_PomodoroRecordCollectionChanged;
            //PomodoroRecordViewModels.CollectionChanged += PomodoroRecordViewModels_CollectionChanged;
        }

        private void PomodoroRecordViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void PomodoroRecordService_PomodoroRecordCollectionChanged(object sender, Models.SystemCollectionChangedNotificationArgs e)
        {
            switch (e.OperationType)
            {
                case Models.CollectionOperationType.ItemAdded:
                    await OnRecordAddedNotificationReceivedAsync(e.AddedItemInnerId.Value);
                    break;
                case CollectionOperationType.ItemRemoved:
                    await OnRecordRemovedNotificationReceivedAsync(e.RemovedItemInnerId.Value);
                    break;
                case CollectionOperationType.ItemPropertyChanged:
                    var changedViewModel = PomodoroRecordViewModels.FirstOrDefault(vm => vm.InnerId == e.ModifiedItemInnerId.Value);
                    await changedViewModel?.RequestUpdateAsync();
                    break;
            }
        }

        async Task OnRecordAddedNotificationReceivedAsync(int addedItemInnerId)
        {
            PomodoroRecordViewModel existingItem = PomodoroRecordViewModels.FirstOrDefault(vm => vm.InnerId == addedItemInnerId);
            if (existingItem != default)
            {
                //已经存在这个项目，对项目进行更新
                await existingItem.RequestUpdateAsync();
            }
            else
            {
                var recordModel = await pomodoroRecordService.QueryPomodoroRecord(addedItemInnerId);
                var viewModel = new PomodoroRecordViewModel(recordModel, pomodoroRecordService);
                await PomodoroRecordViewModels.ThreadSafeAddAsync(viewModel);
            }
        }
        async Task OnRecordRemovedNotificationReceivedAsync(int removedItemInnerId)
        {
            var existingItems = PomodoroRecordViewModels.Where(vm => vm.InnerId == removedItemInnerId).ToList();
            foreach (var item in existingItems)
            {
                await PomodoroRecordViewModels.ThreadSafeRemoveAsync(item);
            }
        }

        [RelayCommand]
        public async Task UpdateAsync()
        {
            var items = await pomodoroRecordService.PullPomodoroRecordsAsync();
            PomodoroRecordViewModels.Clear();
            foreach (var item in items)
            {
                await PomodoroRecordViewModels.ThreadSafeAddAsync(new PomodoroRecordViewModel(item, pomodoroRecordService));
            }
        }

        [RelayCommand]
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
        private ObservableCollection<PomodoroRecordViewModel> pomodoroRecordViewModels = new();
    }
}
