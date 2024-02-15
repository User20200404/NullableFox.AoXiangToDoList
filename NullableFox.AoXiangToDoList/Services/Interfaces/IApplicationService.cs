using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Interfaces
{
    internal interface IApplicationService
    {
        Task RequestExitApplication();
        Task RequestLocalSaveData();
    }
}
