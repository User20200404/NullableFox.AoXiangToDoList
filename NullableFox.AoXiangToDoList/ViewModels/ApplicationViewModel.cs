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
    }
}
