using CommunityToolkit.Mvvm.Input;
using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WinUIEx;

namespace NullableFox.AoXiangToDoList.ViewModels
{
    internal partial class AppConfigurationViewModel : MultiThreadBindableBase
    {
        IConfigurationService configurationService;
        AppConfiguration config;
        public AppConfigurationViewModel(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public Size ApplicationMainWindowSize
        {
            get => config.ApplicationMainWindowSize;
            set => SetProperty(ref config.ApplicationMainWindowSize, value);
        }

        public WindowState ApplicationMainWindowState
        {
            get => config.ApplicationMainWindowState;
            set => SetProperty(ref config.ApplicationMainWindowState, value);
        }

        [RelayCommand]
        public Task SaveAsync() => configurationService.SaveAsync(config);

        [RelayCommand]
        public async Task LoadAsync() => config = await configurationService.LoadAsync().ConfigureAwait(false);
    }
}