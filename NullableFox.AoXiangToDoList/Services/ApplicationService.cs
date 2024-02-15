using NullableFox.AoXiangToDoList.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services
{
    internal class ApplicationService : IApplicationService
    {
        INetworkService networkService;
        public ApplicationService(INetworkService service)
        {
            this.networkService = service;
        }

        public async Task RequestExitApplication()
        {
            await networkService.RequestAsync(Transmission.RequestType.ExitApplication, string.Empty);
        }

        public async Task RequestLocalSaveData()
        {
            await networkService.RequestAsync(Transmission.RequestType.SaveSystemData, string.Empty);
        }
    }
}
