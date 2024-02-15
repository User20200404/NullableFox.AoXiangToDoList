using Microsoft.Extensions.DependencyInjection;
using NullableFox.AoXiangToDoList.Exceptions;
using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.Utilities;
using NullableFox.AoXiangToDoList.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services
{
    internal class UserService : IUserService
    {
        INetworkService NetworkService => App.Current.ServiceProvider.GetRequiredService<INetworkService>();
        public async Task<User> PullUserModelAsync()
        {
            var service = NetworkService;
            var result = await service.RequestAsync(Transmission.RequestType.GetCurrentUser, string.Empty);
            if (result.Status == Transmission.ResponseStatus.Success)
            {
                return JsonHelper.ObjectFromJsonString<User>(result.Content);
            }
            return null;
        }

        public async Task PushUserAsync(UserInfo newUserInfo)
        {
            var result = await NetworkService.RequestAsync(Transmission.RequestType.ModifyUserInfo, JsonHelper.ToJsonString(newUserInfo));
            ExceptionHelper.ThrowOnTransFailure(result);
        }
    }
}
