using Microsoft.Extensions.DependencyInjection;
using NullableFox.AoXiangToDoList.Exceptions;
using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.Services.Transmission;
using NullableFox.AoXiangToDoList.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableFox.AoXiangToDoList.Services
{
    internal class ToDoWorkItemService : IToDoWorkItemService
    {
        INetworkService networkService;
        INotificationService notificationService;

        public event EventHandler<SystemCollectionChangedNotificationArgs> ToDoCollectionChanged;

        public async Task<int> CreateToDoWorkAsync(ToDoWorkItem toDoWorkItem)
        {
            var packet = await networkService.RequestAsync(Transmission.RequestType.CreateToDoWork, JsonHelper.ToJsonString(toDoWorkItem));
            ThrowOnFailure(packet,"创建待办事项时发生错误。");
            return int.Parse(packet.Content);
        }

        public async Task DeleteToDoWorkItemAsync(int innerId)
        {
            var packet = await networkService.RequestAsync(Transmission.RequestType.DeleteToDoWork, innerId.ToString());
            ThrowOnFailure(packet,"删除待办事项时发生错误");
        }

        public async Task EditToDoWorkItemAsync(ToDoWorkItem toDoWorkItem)
        {
            var packet = await networkService.RequestAsync(Transmission.RequestType.EditToDoWork, JsonHelper.ToJsonString(toDoWorkItem));
            ThrowOnFailure(packet,"编辑待办事项时发生错误");
        }

        public async Task<List<ToDoWorkItem>> PullToDoWorkItemsAsync()
        {
            var packet = await networkService.RequestAsync(Transmission.RequestType.EnumerateToDoWorkList, "");
            ThrowOnFailure(packet,"拉取待办事项时发生错误");
            return JsonHelper.ObjectFromJsonString<List<ToDoWorkItem>>(packet.Content);
        }

        public async Task<ToDoWorkItem> QueryToDoWorkItemAsync(int innerId)
        {
            var packet = await networkService.RequestAsync(Transmission.RequestType.QueryToDoWork, innerId.ToString());
            ThrowOnFailure(packet, "查询待办事项时发生错误");
            return JsonHelper.ObjectFromJsonString<ToDoWorkItem>(packet.Content);
        }

        public void ThrowOnFailure(ResponsePacket response,string title = "发生未指定错误")
        {
            ExceptionHelper.ThrowOnTransFailure(response, title);
        }

        public ToDoWorkItemService(INetworkService networkService,INotificationService notificationService)
        {
            this.networkService = networkService;
            this.notificationService = notificationService;
            this.notificationService.ToDoCollectionChanged += (s, e) => ToDoCollectionChanged?.Invoke(s, e);
        }
    }
}
