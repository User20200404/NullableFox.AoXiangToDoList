using NullableFox.AoXiangToDoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services.Interfaces
{
    internal interface IConfigurationService
    {
        public Task SaveAsync(AppConfiguration config);
        public Task<AppConfiguration> LoadAsync();
    }
}
