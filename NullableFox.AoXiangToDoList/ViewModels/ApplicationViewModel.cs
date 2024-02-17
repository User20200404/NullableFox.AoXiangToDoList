using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.ViewModels
{
    internal partial class ApplicationViewModel : MultiThreadBindableBase
    {
        private IApplicationService appService;
        private bool isOnMiniFocusView;

        public event EventHandler GoToMiniFocusViewRequested;
        public event EventHandler GoToNormalViewRequested;
        public event EventHandler ExitUIRequested;

        [ObservableProperty]
        bool isExiting;


        public ApplicationViewModel(IApplicationService service)
        {
            this.appService = service;
        }

        [RelayCommand]
        public async Task ExitApplication()
        {
            await appService.RequestExitApplication();
        }
        [RelayCommand]
        public async Task LocalSaveData()
        {
            await appService.RequestLocalSaveData();
        }

        [RelayCommand]
        public void GoToMiniFoucsView()
        {
            GoToMiniFocusViewRequested?.Invoke(this, EventArgs.Empty);
            isOnMiniFocusView = true;
        }

        [RelayCommand]
        public void GoToNormalView()
        {
            GoToNormalViewRequested?.Invoke(this, EventArgs.Empty);
            isOnMiniFocusView = false;
        }

        [RelayCommand]
        public void ToggleFocusView()
        {
            if (isOnMiniFocusView)
            {
                GoToNormalView();
            }
            else
            {
                GoToMiniFoucsView();
            }
        }

        [RelayCommand]
        public void ExitUI()
        {
            ExitUIRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
